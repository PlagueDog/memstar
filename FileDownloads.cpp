#include <winsock2.h>
#include <ws2tcpip.h>
#include <string>
#include <fstream>
#include <iostream>
#include <filesystem>
#include <cstdarg>
#include "zlib/zlib.h" // For crc32

#include "Fear.h"
#include "Console.h"
#include "Windows.h"
#include "Patch.h"

// Function to compute CRC32 of a file
uint32_t ComputeFileCRC(const std::string& filepath) {
    std::ifstream file(filepath, std::ios::binary);
    if (!file.is_open()) return 0;

    uint32_t crc = crc32(0L, Z_NULL, 0);
    char buffer[4096];
    while (file.read(buffer, sizeof(buffer))) {
        crc = crc32(crc, (const Bytef*)buffer, file.gcount());
    }
    // Handle last partial read
    if (file.gcount() > 0)
        crc = crc32(crc, (const Bytef*)buffer, file.gcount());

    return crc;
}

#pragma comment(lib, "Ws2_32.lib")

// Helper function to extract filename from a path
std::string GetFilenameFromPath(const std::string& filepath) {
    size_t pos1 = filepath.find_last_of("/\\");
    if (pos1 != std::string::npos) {
        return filepath.substr(pos1 + 1);
    }
    return filepath; // No directory path found, return original
}

const char* getFileNameCStr(const std::string& filePath) {
    size_t pos = filePath.find_last_of("/\\");
    if (pos == std::string::npos) {
        return filePath.c_str();
    }
    return filePath.substr(pos + 1).c_str();
}

BuiltInFunction("stripFilePath", _stripfilepath)
{
    if (argc != 1)
    {
        Console::echo("%s( file );", self);
        return 0;
    }
    return getFileNameCStr(argv[0]);
}

struct DownloadParams {
    std::string serverIP;
    int port;
    std::string filename;
    std::string savePath;
};

DWORD WINAPI DownloadThread(LPVOID param) {
    DownloadParams* p = (DownloadParams*)param;

    // Check if file exists and compute CRC
    uint32_t localCRC = 0;
    std::string filename = p->filename;
    std::string fullPath = p->savePath + GetFilenameFromPath(filename);

    if (std::filesystem::exists(fullPath)) {
        localCRC = ComputeFileCRC(fullPath);
        //Console::echo("Local file exists. CRC: %u", localCRC);
    }
    else {
        //Console::echo("Local file does not exist. Will download.");
    }

    //Console::echo("Initializing Winsock...");
    WSADATA wsaData;
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
        //Console::echo("WSAStartup failed");
        delete p;
        return 1;
    }

    //Console::echo("Creating socket...");
    SOCKET sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

    if (sock == INVALID_SOCKET) {
        //Console::echo("Socket creation failed: %ld", WSAGetLastError());
        WSACleanup();
        delete p;
        return 1;
    }

    //Console::echo("Connecting to %s:%d...", p->serverIP.c_str(), p->port);
    sockaddr_in serverAddr{};
    serverAddr.sin_family = AF_INET;
    inet_pton(AF_INET, p->serverIP.c_str(), &serverAddr.sin_addr);
    serverAddr.sin_port = htons(p->port);

    if (connect(sock, (sockaddr*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR) {
        //Console::echo("Connection failed: %ld", WSAGetLastError());
        closesocket(sock);
        WSACleanup();
        delete p;
        return 1;
    }

    // Connect to server (existing code)
// Send CRC + filename request
    std::string request = "CRC:" + std::to_string(localCRC) + "\n" + filename;
    int sendResult = send(sock, request.c_str(), request.size(), 0);
    if (sendResult == SOCKET_ERROR) {
        //Console::echo("Failed to send CRC request: %ld", WSAGetLastError());
        closesocket(sock);
        WSACleanup();
        delete p;
        return 1;
    }

    // Wait for server response
    char responseBuffer[256];
    int respSize = recv(sock, responseBuffer, sizeof(responseBuffer) - 1, 0);
    if (respSize <= 0) {
        //Console::echo("Failed to receive server response: %ld", WSAGetLastError());
        closesocket(sock);
        WSACleanup();
        delete p;
        return 1;
    }
    responseBuffer[respSize] = '\0';

    std::string serverResponse(responseBuffer);
    if (serverResponse.find("UP-TO-DATE") == 0) {
        //Console::echo("File is up-to-date. No download needed.");
        char eval2[255];
        sprintf(eval2, "Net::LoadServerMod('%s');", fullPath.c_str());
        Console::eval(eval2);
        closesocket(sock);
        WSACleanup();
        delete p;
        return 0;
    }
    else if (serverResponse.find("SEND") == 0) {
        //Console::echo("Server indicates to send file. Proceeding...");
    }

    //Console::echo("Connected to server!");

    //Console::echo("Filename sent, awaiting data...");

    // Prepare to save file
    //std::string fullPath = p->savePath + "/" + p->filename;

    //Directorys must be stripped from the fileName
    std::ofstream outFile(fullPath, std::ios::binary);
    if (!outFile.is_open()) {
        //Console::echo("Failed to open file for writing: %s", fullPath.c_str());
        closesocket(sock);
        WSACleanup();
        delete p;
        return 1;
    }
    Console::echo("Downloading to: %s", fullPath.c_str());

    // Receive data loop
    //char buffer[1024];
    char buffer[65535];
    int recvSize;
    while ((recvSize = recv(sock, buffer, sizeof(buffer), 0)) > 0) {
        std::string data(buffer, recvSize);
        // Check for server error message
        if (data.find("ERROR") == 0) {
            Console::echo("Server error: %s", data.c_str());
            outFile.close();
            closesocket(sock);
            WSACleanup();
            delete p;
            return 1;
        }
        outFile.write(buffer, recvSize);
       // Console::echo("Received %d bytes...", recvSize);

        //This runs so fast our client script doesn't work correctly
        //char eval1[255];
        //sprintf(eval1, "modloader::updateProgressBar('%s', %d);", GetFilenameFromPath(p->filename), recvSize);
        //Console::eval(eval1);
    }

    if (recvSize == 0) {
        Console::echo("Download complete: %s", fullPath.c_str());
    }
    else {
        Console::echo("Receive failed: %ld", WSAGetLastError());
        outFile.close();
        closesocket(sock);
        WSACleanup();
        delete p;
        return 1;
    }

    outFile.close();
    //Console::echo("File saved successfully: %s", fullPath.c_str());
    char eval2[255];
    sprintf(eval2, "Net::LoadServerMod('%s');", fullPath.c_str());
    Console::eval(eval2);
    closesocket(sock);
    WSACleanup();
    delete p; // Clean up params after done
    return 0;
}

void StartDownload(const std::string& serverIP, int port, const std::string& filename, const std::string& savePath) {
    // Allocate and set parameters
    DownloadParams* params = new DownloadParams{ serverIP, port, filename, savePath };

    // Create thread
    HANDLE hThread = CreateThread(NULL, 0, DownloadThread, params, 0, NULL);
    if (hThread) {
        CloseHandle(hThread); // Detach thread, won't wait for it
    }
    else {
        Console::echo("Failed to create download thread");
        delete params;
    }
}

#include <thread>
#include <atomic>

// Server thread data
struct ServerContext {
    int port;
    bool running;
    HANDLE threadHandle;
    HANDLE shutdownEvent; // To signal shutdown
};

// Forward declaration
DWORD WINAPI ServerThread(LPVOID param);
DWORD WINAPI ClientHandler(LPVOID clientSocket);

// Exported functions
extern "C" {

    __declspec(dllexport) void StartServer(int port);
    __declspec(dllexport) void StopServer();

}

// Global server context
static ServerContext g_serverCtx = { 0 };

// Implementation
void StartServer(int port) {
    if (g_serverCtx.running) {
        Console::echo("Nova: File server already running");
        return;
    }

    g_serverCtx.port = port;
    g_serverCtx.running = true;
    g_serverCtx.shutdownEvent = CreateEvent(NULL, TRUE, FALSE, NULL);

    g_serverCtx.threadHandle = CreateThread(
        NULL,
        0,
        ServerThread,
        &g_serverCtx,
        0,
        NULL
    );
    Console::echo("Nova: File server thread started");
}

void StopServer() {
    if (!g_serverCtx.running) return;

    g_serverCtx.running = false;
    SetEvent(g_serverCtx.shutdownEvent);
    WaitForSingleObject(g_serverCtx.threadHandle, INFINITE);
    CloseHandle(g_serverCtx.threadHandle);
    CloseHandle(g_serverCtx.shutdownEvent);
    g_serverCtx.threadHandle = NULL;
    g_serverCtx.shutdownEvent = NULL;
    Console::echo("File Server stopped");
}

// Thread function that runs the server
DWORD WINAPI ServerThread(LPVOID param) {
    ServerContext* ctx = (ServerContext*)param;
    WSADATA wsaData;
    WSAStartup(MAKEWORD(2, 2), &wsaData);

    SOCKET listenSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    sockaddr_in service{};
    service.sin_family = AF_INET;
    //service.sin_addr.s_addr = htonl(INADDR_ANY);
    service.sin_addr.s_addr = INADDR_ANY;
    service.sin_port = htons(ctx->port);

    if (bind(listenSocket, (SOCKADDR*)&service, sizeof(service)) == SOCKET_ERROR) {
        Console::echo("Nova: Bind failed: %ld", WSAGetLastError());
        closesocket(listenSocket);
        WSACleanup();
        return 1;
    }

    if (listen(listenSocket, SOMAXCONN) == SOCKET_ERROR) {
        Console::echo("Nova: Listen failed: %ld", WSAGetLastError());
        closesocket(listenSocket);
        WSACleanup();
        return 1;
    }

    Console::echo("Nova: File server listening on port: %d", ctx->port);

    while (WaitForSingleObject(ctx->shutdownEvent, 0) != WAIT_OBJECT_0) {
        // Accept new client
        SOCKET clientSock = accept(listenSocket, NULL, NULL);
        if (clientSock == INVALID_SOCKET) {
            int err = WSAGetLastError();
            if (err == WSAEINTR || err == WSAEWOULDBLOCK) {
                Sleep(100); // Try again
                continue;
            }
            Console::echo("Nova: Accept failed: %ld", err);
            break;
        }

        // Create thread for client
        CreateThread(NULL, 0, ClientHandler, (LPVOID)clientSock, 0, NULL);
    }

    closesocket(listenSocket);
    WSACleanup();
    return 0;
}

// Client handler thread
DWORD WINAPI ClientHandler(LPVOID clientSocketVoid) {
    SOCKET sock = (SOCKET)clientSocketVoid;
    char buffer[1024];
    int recvSize = recv(sock, buffer, sizeof(buffer) - 1, 0);
    if (recvSize <= 0) {
        closesocket(sock);
        return 0;
    }

    buffer[recvSize] = '\0'; // null-terminate

    std::string request(buffer);

    // Check if this is a CRC check request
    if (request.find("CRC:") == 0) {
        size_t newlinePos = request.find('\n');
        if (newlinePos == std::string::npos) {
            // Malformed request
            closesocket(sock);
            return 0;
        }

        std::string crcStr = request.substr(4, newlinePos - 4);
        uint32_t clientCRC = std::stoul(crcStr);
        std::string filename = request.substr(newlinePos + 1);

        // Security check: prevent directory traversal
        if (filename.find(":") != -1 || filename.find("..") != -1) {
            Console::echo("Client attempted to request file outside of allowed directory (%s)", filename.c_str());
            std::string errMsg = "ERROR: Invalid filename";
            send(sock, errMsg.c_str(), errMsg.size(), 0);
            closesocket(sock);
            return 0;
        }

        //Console::echo("Client CRC check for file: %s (client CRC: %u)", filename.c_str(), clientCRC);

        // Compute server CRC for the requested file
        uint32_t serverCRC = 0;
        std::string fullPath = filename; // Adjust path as needed
        {
            std::ifstream file(fullPath, std::ios::binary);
            if (file.is_open()) {
                serverCRC = crc32(0L, Z_NULL, 0);
                char buf[4096];
                while (file.read(buf, sizeof(buf))) {
                    serverCRC = crc32(serverCRC, (const Bytef*)buf, file.gcount());
                }
                // Last partial read
                if (file.gcount() > 0) {
                    serverCRC = crc32(serverCRC, (const Bytef*)buf, file.gcount());
                }
            }
            else {
                // File not found, send error
                std::string errMsg = "ERROR: File not found";
                send(sock, errMsg.c_str(), errMsg.size(), 0);
                closesocket(sock);
                return 0;
            }
        }

        // Compare CRCs
        if (clientCRC == serverCRC) {
            // Files are identical
            std::string response = "UP-TO-DATE";
            send(sock, response.c_str(), response.size(), 0);
            //Console::echo("File %s is up-to-date, not sending.", filename.c_str());
            closesocket(sock);
            return 0;
        }
        else {
            // Files differ, send signal to send file
            std::string response = "SEND";
            send(sock, response.c_str(), response.size(), 0);
            //Console::echo("Files differ. Sending file: %s", filename.c_str());

            // Proceed to send the file data
            std::ifstream file(fullPath, std::ios::binary);
            if (!file.is_open()) {
                std::string errMsg = "ERROR: Unable to open file.";
                send(sock, errMsg.c_str(), errMsg.size(), 0);
                closesocket(sock);
                return 0;
            }

            char sendBuffer[4096];
            while (!file.eof()) {
                file.read(sendBuffer, sizeof(sendBuffer));
                std::streamsize bytesRead = file.gcount();
                send(sock, sendBuffer, bytesRead, 0);
            }
            file.close();
            closesocket(sock);
            return 0;
        }
    }
}

namespace NovaFileServer
{
    BuiltInFunction("Nova::requestServerFile", _novarequestserverfile)
    {
        if(argc != 4)
        {
            Console::echo("%s(ip, port, fileName, downloadDirectory);",self);
            return 0;
        }
        std::string path = argv[2];
        if (path.find(":") != -1 || path.find("..") != -1)
        {
            Console::echo("Cannot request files outside of the game directory.");
            return 0;
        }
        //StartDownload("127.0.0.1", 27015, "defaultPrefs.cs", ".\\mods");
        StartDownload(argv[0], atoi(argv[1]), argv[2], argv[3]);
        return "true";
    }

    bool is_number(const std::string& s)
    {
        std::string::const_iterator it = s.begin();
        while (it != s.end() && std::isdigit(*it)) ++it;
        return !s.empty() && it == s.end();
    }

    BuiltInFunction("Nova::startFileServer", _novastartfileserver)
    {
        if (argc != 1 || !is_number(argv[0]))
        {
            Console::echo("%s( port );", self);
            return 0;
        }

        StartServer(atoi(argv[0]));
        return "true";
    }

    //CURRENTLY BROKEN
    //BuiltInFunction("Nova::stopFileServer", _novastopfileserver)
    //{
    //    StopServer();
    //    Console::echo("Nova: Shutting down file server.");
    //    return "true";
    //}

    // Test function using Console::echo() without "\n"
    void TestConnect(const std::string& serverIP, int port) {
        WSADATA wsaData;
        SOCKET sock = INVALID_SOCKET;

        if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
            //Console::echo("WSAStartup failed");
            return;
        }

        sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
        if (sock == INVALID_SOCKET) {
            //Console::echo("Failed to create socket");
            WSACleanup();
            return;
        }

        sockaddr_in serverAddr{};
        serverAddr.sin_family = AF_INET;
        inet_pton(AF_INET, serverIP.c_str(), &serverAddr.sin_addr);
        serverAddr.sin_port = htons(port);

        if (connect(sock, (sockaddr*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR) {
            //Console::echo("Connection failed");
            closesocket(sock);
            WSACleanup();
            return;
        }

        Console::echo("Connected to server at %s:%d", serverIP.c_str(), port);

        // Send a test message (e.g., filename or string)
        std::string testMsg = "testfile.txt";
        send(sock, testMsg.c_str(), static_cast<int>(testMsg.size()), 0);

        closesocket(sock);
        WSACleanup();
    }

    BuiltInFunction("Nova::testConnect", _novatestconnect)
    {
        if (argc != 2)
        {
            Console::echo("%s(ip, port);", self);
            return 0;
        }
        TestConnect(argv[0], atoi(argv[1]));
        return "true";
    }

    struct Init {
        Init() {
        }
    } init;
}
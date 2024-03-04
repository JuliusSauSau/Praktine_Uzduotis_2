#include <iostream>
#include <fstream>
#include <string>

using namespace std;

string encrypt(const string& plaintext, const string& key)
{
    string ciphertext;
    for (size_t i = 0; i < plaintext.length(); ++i)
    {
                char encryptedChar = plaintext[i] ^ key[i % key.length()];
        if (encryptedChar < 32 || encryptedChar > 126)
        {
                        encryptedChar = encryptedChar % 95 + 32;
        }
        ciphertext += encryptedChar;
    }
    return ciphertext;
}

string decrypt(const string& ciphertext, const string& key)
{
    string plaintext;
    for (size_t i = 0; i < ciphertext.length(); ++i)
    {
        plaintext += ciphertext[i] ^ key[i % key.length()];
    }
    return plaintext;
}

void saveToFile(const string& text, const string& filename)
{
    ofstream outFile(filename);
    if (outFile.is_open())
    {
        outFile << text;
        cout << "Text saved to file: " << filename << endl;
        cout << endl;
    }
    else
    {
        cout << "Unable to open file: " << filename << endl;
    }
    outFile.close();
}

string readFromFile(const string& filename)
{
    ifstream inFile(filename);
    string text, line;
    if (inFile.is_open())
    {
        while (getline(inFile, line))
        {
            text += line;
        }
        cout << "Text read from file: " << filename << endl;
        cout << endl;
    }
    else
    {
        cout << "Unable to open file: " << filename << endl;
    }
    inFile.close();
    return text;
}

int main()
{
    int choice;
    string inputText, key, outputText, filename;
    bool continueLoop = true;

    while (continueLoop)
    {
        cout << "Select operation:" << endl;
        cout << "1. Encrypt" << endl;
        cout << "2. Decrypt" << endl;
        cout << "3. Exit" << endl;
        cout << "Enter choice: ";
        cin >> choice;
        cout << endl;

        switch (choice)
        {
            case 1:
                cout << "Enter plaintext: ";
                cin.ignore();
                getline(cin, inputText);
                cout << endl;

                cout << "Enter key: ";
                getline(cin, key);
                outputText = encrypt(inputText, key);
                cout << endl;

                cout << "Ciphertext: " << outputText << endl;
                cout << endl;

                cout << "Enter filename to save ciphertext: ";
                cin >> filename;

                saveToFile(outputText, filename);
                break;

            case 2:
                cout << "Decrypting from file or manual input?" << endl;
                cout << "1. File" << endl;
                cout << "2. Manual input" << endl;
                cout << "Enter choice: ";
                cin >> choice;
                cout << endl;

                switch (choice)
                {
                    case 1:
                        cout << "Enter filename to read ciphertext: ";
                        cin >> filename;
                        cout << endl;

                        inputText = readFromFile(filename);
                        break;

                    case 2:
                        cout << "Enter ciphertext: ";
                        cin.ignore();
                        cout << endl;

                        getline(cin, inputText);
                        break;

                    default:
                        cout << "Invalid choice" << endl;
                        continue;
                }

                cout << "Enter key: ";
                cin >> key;
                cout << endl;

                outputText = decrypt(inputText, key);
                cout << "Decrypted plaintext: " << outputText << endl;
                cout << endl;
                break;

            case 3:
                continueLoop = false;
                break;

            default:
                cout << "Invalid choice" << endl;
                break;
        }
    }

    return 0;
}

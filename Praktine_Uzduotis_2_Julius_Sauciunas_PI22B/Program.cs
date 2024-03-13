using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class EncryptionSystem
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Encryption System");

            while (true)
            {
                Console.WriteLine("\n1. Encrypt Text");
                Console.WriteLine("2. Decrypt Text");
                Console.WriteLine("3. Save Encrypted Text to File");
                Console.WriteLine("4. Read Encrypted Text from File and Decrypt");
                Console.WriteLine("5. Exit");

                Console.Write("\nEnter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        EncryptText();
                        break;
                    case 2:
                        DecryptText();
                        break;
                    case 3:
                        SaveEncryptedTextToFile();
                        break;
                    case 4:
                        ReadEncryptedTextFromFileAndDecrypt();
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static void EncryptText()
    {
        Console.WriteLine("\n--- Encrypt Text ---");

        Console.Write("Enter the text to encrypt: ");
        string originalText = Console.ReadLine();

        Console.Write("Enter the encryption key: ");
        string key = Console.ReadLine();

        Console.Write("Choose encryption mode (ECB, CBC, CFB): ");
        string mode = Console.ReadLine();

        byte[] encryptedBytes = EncryptStringToBytes(originalText, key, mode);

        Console.WriteLine("\nEncrypted Text: " + Convert.ToBase64String(encryptedBytes));
    }

    static void DecryptText()
    {
        Console.WriteLine("\n--- Decrypt Text ---");

        Console.Write("Enter the text to decrypt: ");
        string encryptedText = Console.ReadLine();

        Console.Write("Enter the encryption key: ");
        string key = Console.ReadLine();

        Console.Write("Choose encryption mode (ECB, CBC, CFB): ");
        string mode = Console.ReadLine();

        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        string decryptedText = DecryptBytesToString(encryptedBytes, key, mode);

        Console.WriteLine("\nDecrypted Text: " + decryptedText);
    }

    static void SaveEncryptedTextToFile()
    {
        Console.WriteLine("\n--- Save Encrypted Text to File ---");

        Console.Write("Enter the text to encrypt and save to file: ");
        string originalText = Console.ReadLine();

        Console.Write("Enter the encryption key: ");
        string key = Console.ReadLine();

        Console.Write("Choose encryption mode (ECB, CBC, CFB): ");
        string mode = Console.ReadLine();

        Console.Write("Enter the filename to save the encrypted text: ");
        string fileName = Console.ReadLine();

        byte[] encryptedBytes = EncryptStringToBytes(originalText, key, mode);

        string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        File.WriteAllBytes(filePath, encryptedBytes);

        Console.WriteLine("\nEncrypted text saved to file: " + filePath);
    }

    static void ReadEncryptedTextFromFileAndDecrypt()
    {
        Console.WriteLine("\n--- Read Encrypted Text from File and Decrypt ---");

        Console.Write("Enter the filename to read encrypted text from: ");
        string fileName = Console.ReadLine();

        if (!File.Exists(fileName))
        {
            Console.WriteLine("File does not exist.");
            return;
        }

        Console.Write("Enter the encryption key: ");
        string key = Console.ReadLine();

        Console.Write("Choose encryption mode (ECB, CBC, CFB): ");
        string mode = Console.ReadLine();

        byte[] encryptedBytes = File.ReadAllBytes(fileName);

        string decryptedText = DecryptBytesToString(encryptedBytes, key, mode);

        Console.WriteLine("\nDecrypted Text: " + decryptedText);
    }

    static byte[] EncryptStringToBytes(string plainText, string key, string mode)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.Mode = GetCipherMode(mode);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }
    }

    static string DecryptBytesToString(byte[] cipherText, string key, string mode)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.Mode = GetCipherMode(mode);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    static CipherMode GetCipherMode(string mode)
    {
        switch (mode.ToUpper())
        {
            case "ECB":
                return CipherMode.ECB;
            case "CBC":
                return CipherMode.CBC;
            case "CFB":
                return CipherMode.CFB;
            default:
                throw new ArgumentException("Invalid cipher mode.");
        }
    }
}

/*
 Convertir de hexadecimal a base64:
 - Representación binaria de hex
 - Agrupación en segmentos de 6 bits
    - Si un segmento no contiene 6 bits se añade padding binario
 - Se mapea a base64
    - Si número total de bits no es múltiplo de 4 se añade padding "="
 */

using System.Text;

using System.Text.RegularExpressions;

ConsoleKey key;

do
{
    Console.Clear();
    
    Console.WriteLine("Introduce aquí tu código HEXADECIMAL: ");

    string? hexCode = Console.ReadLine()?.Trim().ToUpper();

    if (string.IsNullOrEmpty(hexCode)) //(hexCode == null || hexCode.Length == 0)

    {
        Console.WriteLine("La cadena no puede estar vacía.");
        return;
    }

    hexCode = Regex.Replace(hexCode, @"\s+", "");


    if (!Regex.IsMatch(hexCode, "^[0-9a-fA-F]+$"))
    {
        Console.WriteLine($"El código {hexCode} contiene caracteres no válidos.");
    }

    if (hexCode.Length % 2 != 0)
    {
        Console.WriteLine($"La cadena debe contener un número par de caracteres.");
        return;
    }

    string binaryCode = HexToBinary(hexCode);

    byte[] bytesCode = BinaryToBytes(binaryCode);

    string base64Code = BytesToBase64(bytesCode);

    Console.WriteLine($"Tu código hexadecimal: {hexCode}");

    Console.WriteLine($"Tu código en base64: {base64Code}");
    
    Console.WriteLine("Presiona una cualquier tecla para continuar (ESC para salir).");
    key = Console.ReadKey(true).Key;

} while (key != ConsoleKey.Escape);

//// EMPIEZA DEFINICIÓN DE MÉTODOS

string BytesToBase64(byte[] bytes)
{
    const string base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

    StringBuilder sb = new StringBuilder(((bytes.Length + 2) / 3) * 4);

    for (int i = 0; i < bytes.Length; i+=3)

    {
        int b1 = bytes[i];

        int b2 = (i + 1 < bytes.Length) ? bytes[i + 1] : 0;

        int b3 = (i + 2 < bytes.Length) ? bytes[i + 2] : 0;


        int triple = (b1 << 16 | b2 << 8 | b3);

        sb.Append(base64Chars[(triple >> 18) & 63]); //0x3F
        sb.Append(base64Chars[(triple >> 12) & 63]);
        sb.Append(i + 1 < bytes.Length ? base64Chars[(triple >> 6) & 63] : '=');
        sb.Append(i + 2 < bytes.Length ? base64Chars[triple & 63] : '=');
    }

    return sb.ToString();
}


int HexToInt(char c) => c switch
{
    >= '0' and <= '9' => c - '0',
    >= 'A' and <= 'F' => c - 'A' + 10,
    _ => throw new Exception($"Carácter hexadecimal {c} no válido.")
};



string IntToBinary(int digit) // digítos del 0 al 15 de hex
{
    char[] bits = new char[4]; // hex usa 4 bits por carácter

    for (int i = 0; i < 4; i++)
    {
        int exp = 3 - i;
        int power = (int)Math.Pow(2, exp); // 1 << (3 -  i)

        bits[i] = (digit / power) % 2 == 1 ? '1' : '0'; // (digit & (1 << (3 - i))) == 0 ? '0' : '1';

    }
    
    return new string(bits); // constructor String equivalente a [...] foreach (char in s) s+=c; return s;
}

byte[] BinaryToBytes(string binary)
{
    int byteCount = binary.Length / 8;
    byte[] bytes = new byte[byteCount];

    for (int i = 0; i < byteCount; i++)
    {
        int b = 0;

        for (int j = 0; j < 8; j++)
        {
            b = 2 * b + (binary[i * 8 + j] == '1' ? 1 : 0);
        }

        bytes[i] = (byte)b;
    }

    return bytes;
}


string HexToBinary(string hex)

{
    StringBuilder sb = new();

    foreach (char c in hex)
    {
        int digit = HexToInt(c);
        string bits = IntToBinary(digit);
        sb.Append(bits);
    }
    
    return sb.ToString();
}

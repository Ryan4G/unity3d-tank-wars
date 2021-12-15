﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
public class MsgBase
{
    public string protoName = "";
    
    public static byte[] Encode(MsgBase msgBase)
    {
        string s = JsonConvert.SerializeObject(msgBase);

        return System.Text.Encoding.UTF8.GetBytes(s);
    }

    public static MsgBase Decode(string protoName, byte[] bytes, int offset, int count)
    {
        MsgBase msgBase = null;

        try
        {
            string s = System.Text.Encoding.UTF8.GetString(bytes, offset, count);

            Console.WriteLine($"[ Debug ] Receive Proto -> {s}");

            msgBase = (MsgBase)JsonConvert.DeserializeObject(s, Type.GetType(protoName));

        }
        catch (JsonException ex)
        {
            Console.WriteLine($"[ Server ] Decode fail : {ex}");
        }

        return msgBase;
    }

    public static byte[] EncodeName(MsgBase msgBase)
    {
        byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
        short len = (short)nameBytes.Length;
        byte[] bytes = new byte[2 + len];

        bytes[0] = (byte)(len % 256);
        bytes[1] = (byte)(len / 256);

        Array.Copy(nameBytes, 0, bytes, 2, len);

        return bytes;
    }

    public static string DecodeName(byte[] bytes, int offset, out int count)
    {
        count = 0;

        if (offset + 2 > bytes.Length)
        {
            return "";
        }

        short len = (short)((bytes[offset + 1] << 8) | bytes[offset]);

        if (offset + 2 + len > bytes.Length)
        {
            return "";
        }

        count = 2 + len;

        string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);

        return name;
    }
}

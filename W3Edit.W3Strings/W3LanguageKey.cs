﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W3Edit.W3Strings
{
    public class W3LanguageKey
    {
        public W3LanguageKey(UInt32 key, string language)
        {
            this.Key = key;
            this.Language = language;
        }

        public uint Key { get; set; }
        public string Language { get; set; }


        private static Dictionary<UInt32, W3LanguageKey> languageKeys = new Dictionary<UInt32, W3LanguageKey>() {
            { 0x00000000, new W3LanguageKey(0x00000000, "__") },
            { 0x83496237, new W3LanguageKey(0x73946816, "pl") },
            { 0x43975139, new W3LanguageKey(0x79321793, "en") },
            { 0x75886138, new W3LanguageKey(0x42791159, "de") },
            { 0x45931894, new W3LanguageKey(0x12375973, "it") },
            { 0x23863176, new W3LanguageKey(0x75921975, "fr") },
            { 0x24987354, new W3LanguageKey(0x21793217, "cz") },
            { 0x18796651, new W3LanguageKey(0x42387566, "es") },
            { 0x18632176, new W3LanguageKey(0x16875467, "zh") },
            { 0x63481486, new W3LanguageKey(0x42386347, "ru") },
            { 0x42378932, new W3LanguageKey(0x67823218, "hu") },
            { 0x54834893, new W3LanguageKey(0x59825646, "jp") },
        };

        public static W3LanguageKey Get(UInt32 key)
        {
            if (languageKeys.ContainsKey(key))
                return languageKeys[key];
            throw new Exception("Missing language key: " + key);
        }
    }
}

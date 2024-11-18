using System.Collections.Generic;

namespace Hack
{
    public class Generator
    {
        private static Dictionary<string, string> destTable = new(){
            {  "M", "001"},
            {  "D", "010"},
            { "MD", "011"},
            {  "A", "100"},
            { "AM", "101"},
            { "AD", "110"},
            {"AMD", "111"},
        };

        private static Dictionary<string, string> compTable = new(){
            {  "0", "0101010"},
            {  "1", "0111111"},
            { "-1", "0111010"},
            {  "D", "0001100"},
            {  "A", "0110000"},
            {  "M", "1110000"},
            { "!D", "0001101"},
            { "!A", "0110001"},
            { "!M", "1110001"},
            { "-D", "0001111"},
            { "-A", "0110011"},
            { "-M", "1110011"},
            {"D+1", "0011111"},
            {"A+1", "0110111"},
            {"M+1", "1110111"},
            {"D-1", "0001110"},
            {"A-1", "0110010"},
            {"M-1", "1110010"},
            {"D+A", "0000010"},
            {"D+M", "1000010"},
            {"D-A", "0010011"},
            {"D-M", "1010011"},
            {"A-D", "0000111"},
            {"M-D", "1000111"},
            {"D&A", "0000000"},
            {"D&M", "1000000"},
            {"D|A", "0010101"},
            {"D|M", "1010101"},
        };

        private static Dictionary<string, string> jumpTable = new(){
            {"JGT", "001"},
            {"JEQ", "010"},
            {"JGE", "011"},
            {"JLT", "100"},
            {"JNE", "101"},
            {"JLE", "110"},
            {"JMP", "111"},
        };

        public static string Dest(string? field)
        {
            if (field == null) return "000";
            return destTable.GetValueOrDefault(field, "000");
        }

        public static string Comp(string? field)
        {
            if (field == null) return "0000000";
            return compTable.GetValueOrDefault(field, "0000000");
        }

        public static string Jump(string? field)
        {
            if (field == null) return "000";
            return jumpTable.GetValueOrDefault(field, "000");
        }
    }
}
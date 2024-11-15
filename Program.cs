using System;
using System.IO;
using System.Text.Json;

class TimeSignal {
    static void Main() {
        string path = "time.json";

        try {
            string content = File.ReadAllText(path);
            Console.WriteLine(content);
        } catch (FileNotFoundException) {
            int[] newContent = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24];
        }
    }
}

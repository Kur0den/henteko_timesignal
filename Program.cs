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
            int[] newContent = Enumerable.Range(0, 24).ToArray();
        }
    }
}

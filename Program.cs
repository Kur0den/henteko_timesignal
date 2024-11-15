using System.Text;
using System.Text.Json;


class TimeSignal {
    static readonly string timePath = "./time.json";  // 時刻ファイルのパス
    static readonly string configPath = "./config.json";  // 設定ファイルのパス

    static Dictionary<string, string>? config = new Dictionary<string, string>();  // 設定ファイルの内容を格納する変数


    // 新しい時刻ファイルを作成する関数
    static void CreateNewContent() {
        Console.WriteLine("Creating new file");
        int[] newContent = Enumerable.Range(0, 24).ToArray();  // LINQを使用して0から23までの配列を作成
        string jsonContent = JsonSerializer.Serialize(newContent);  // 配列をシリアライズ
        File.WriteAllText(timePath, jsonContent);  // ファイルに書き込み
    }

    // POSTリクエストを送信するための関数
    static async Task PostRequest(int time) {
        using (HttpClient client = new HttpClient()) {
            try {
                // POSTリクエストの送信
                string content = $"$[tada.speed=0s ──────{GetTimeEmoji(time)}{time}時{GetTimeEmoji(time)}──────]";  // 送信する内容
                var bodyContent = new StringContent(@$"{{""i"":""{config["token"]}"", ""text"": ""{content}""}}", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"https://{config["instance"]}/api/notes/create", bodyContent);
                response.EnsureSuccessStatusCode();  // エラーがある場合は例外を投げる
            } catch (HttpRequestException e) {
                Console.WriteLine($"Request error: {e.Message}");
            }
        }
    }

    // 時刻に対応する絵文字を取得する関数
    static string GetTimeEmoji(int Time) {
        return Time switch {
            0 => "🕛",
            1 => "🕐",
            2 => "🕑",
            3 => "🕒",
            4 => "🕓",
            5 => "🕔",
            6 => "🕕",
            7 => "🕖",
            8 => "🕗",
            9 => "🕘",
            10 => "🕙",
            11 => "🕚",
            12 => "🕛",
            13 => "🕐",
            14 => "🕑",
            15 => "🕒",
            16 => "🕓",
            17 => "🕔",
            18 => "🕕",
            19 => "🕖",
            20 => "🕗",
            21 => "🕘",
            22 => "🕙",
            23 => "🕚",
            _ => "🕛"
        };
    }

    // めいん
    static void Main() {

        int[]? remainingTime;


        // 設定ファイルから設定を取得
        try {
            string content = File.ReadAllText(configPath);  // ファイルの内容を取得
            config = JsonSerializer.Deserialize<Dictionary<string, string>>(content); // ファイルの内容をjsonからデシリアライズ
            if (config is null) {
                throw new Exception("config is null");  // nullの場合は例外を投げる
            }
        } catch (FileNotFoundException) {
                throw new Exception("File not found"); // ファイルが存在しない場合は例外を投げる
        }



        // その日にまだ使用していない時刻をファイルから取得
        do {
            try {

                string content = File.ReadAllText(timePath);  // ファイルの内容を取得
                remainingTime = JsonSerializer.Deserialize<int[]>(content);  // ファイルの内容をjsonからデシリアライズ
                if (remainingTime != null) {  // nullかどうかを判断、nullでない場合はループを抜ける
                    break;
                }
                Console.WriteLine("remainingTime is null");
            } catch (FileNotFoundException) {
                Console.WriteLine("File not found");
            }
            CreateNewContent();  // nullな場合/ファイルが存在しない場合は新しいファイルを作成
        } while (true); // infinite loop

    Random rand = new Random();
    int randomTime = remainingTime.OrderBy(x => rand.Next()).First();  // ランダムな要素を取得
    remainingTime = remainingTime.Where(x => x != randomTime).ToArray();  // 取得した要素を削除
    Console.WriteLine($"Random element: {randomTime}");


    PostRequest(randomTime).Wait();  // POSTリクエストを送信



    if (remainingTime.Length == 0) {
        CreateNewContent();  // 要素がなくなった場合は新しいファイルを作成
    } else {
        string jsonContent = JsonSerializer.Serialize(remainingTime);  // 残りの要素をシリアライズ
        File.WriteAllText(timePath, jsonContent);  // ファイルに書き込み
        Console.WriteLine($"Remaining time: {string.Join(", ", remainingTime)}");
    }

    }
}

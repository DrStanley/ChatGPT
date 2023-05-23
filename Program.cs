using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace ChatGPT
{
    class Program
    {

        static async Task Main()
        {
            // Set up your OpenAI API credentials
            string apiKey = "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";
            string apiUrl = "https://api.openai.com/v1/engines/text-davinci-003/completions";

            // Main loop for the chatbot
            Console.WriteLine("ChatGPT: Hello! How can I assist you today?");
            while (true)
            {
                Console.Write("User: ");
                string userMessage = Console.ReadLine();

                if (userMessage.ToLower() == "bye")
                {
                    Console.WriteLine("ChatGPT: Goodbye!");
                    break;
                }

                string botResponse = await SendMessageAsync(userMessage, apiKey, apiUrl);
                Console.WriteLine("ChatGPT: " + botResponse);
            }
        }
        // Function to send a user message and receive a bot response
        async static Task<string> SendMessageAsync(string message, string apiKey, string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                string requestBody = $"{{ \"prompt\": \"{message}\", \"max_tokens\": 50 }}";
                HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                // Check if the API response is valid
                if (response.IsSuccessStatusCode)
                {
                    // Extract the bot response from the API response
                    dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                    if (jsonResponse != null && jsonResponse.choices != null && jsonResponse.choices.Count > 0)
                    {
                        string botResponse = jsonResponse.choices[0].text;
                        return botResponse.Trim();
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid API response.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: API request failed with status code {response.StatusCode}.");
                }

                return string.Empty;
            }
        }
    }
}

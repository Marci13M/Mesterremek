using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CareCompass.Bejelentkezés;
using System.Windows.Forms;

namespace CareCompass
{
    public class ApiClient
    {
        public static string BaseUrl = "http://localhost/mesterremek"; //alap url

        private static readonly SemaphoreSlim TokenRefreshSemaphore = new SemaphoreSlim(1, 1); // Aszinkron lock

        //GET metódusok kezelése 
        public static async Task<dynamic> FetchData(string endpoint)
        {
            try
            {
                string apiUrl = $"{BaseUrl}{endpoint}";
                Debug.WriteLine($"API hívás: {apiUrl}");

                using (var client = new HttpClient())
                {
                    if (Token.ExpiresAt <= DateTime.Now)
                    {
                        await TokenRefreshSemaphore.WaitAsync();
                        try
                        {
                            if (Token.ExpiresAt <= DateTime.Now)
                            {
                                Debug.WriteLine("Lejárt token észlelve, új token kérése...");
                                await RefreshTokenAsync();
                            }
                        }
                        finally
                        {
                            TokenRefreshSemaphore.Release();
                        }
                    }

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Access_token);

                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    string result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Kapott válasz: {result}");

                    dynamic responseData = JsonConvert.DeserializeObject(result);

                    if (responseData != null && responseData.success == false)
                    {
                        string errorMessage = responseData.error?.message ?? "Ismeretlen hiba történt.";
                        Debug.WriteLine($"Hiba: {errorMessage}");
                        MessageBox.Show($"HIBA: {errorMessage}");
                        return null; // Kivétel dobása helyett null visszaadása
                    }

                    if (responseData != null && responseData.error != null && responseData.error.details != null && responseData.error.details.token_detail == false)
                    {
                        await TokenRefreshSemaphore.WaitAsync();
                        try
                        {
                            if (Token.ExpiresAt <= DateTime.Now)
                            {
                                Debug.WriteLine("Lejárt token észlelve a válaszban, új token kérése...");
                                await RefreshTokenAsync();
                            }
                        }
                        finally
                        {
                            TokenRefreshSemaphore.Release();
                        }

                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Access_token);
                        response = await client.GetAsync(apiUrl);
                        result = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Új token után kapott válasz: {result}");
                        responseData = JsonConvert.DeserializeObject(result);
                    }

                    if (responseData == null || responseData.success == false)
                    {
                        string errorMessage = responseData?.error?.message ?? "Sikertelen válasz vagy üres adat.";
                        Debug.WriteLine($"Hiba: {errorMessage}");
                        MessageBox.Show($"HIBA: {errorMessage}");
                        return null; // Kivétel dobása helyett null visszaadása
                    }

                    return responseData.data;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Kivétel: {ex.Message}");
                MessageBox.Show($"HIBA: {ex.Message}");
                return null;
            }
        }

        //Token megújítás
        public static async Task RefreshTokenAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var refreshData = new
                    {
                        user_id = GlobalData.UserId
                    };
                    string jsonData = JsonConvert.SerializeObject(refreshData);

                    var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/v1/api.php/auth/refreshToken")
                    {
                        Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
                    };

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Access_token);

                    HttpResponseMessage response = await client.SendAsync(request);
                    string responseText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic responseData = JsonConvert.DeserializeObject(responseText);
                        if (responseData != null && responseData.success == true)
                        {
                            Token.Access_token = responseData.data.token.access_token;
                            Token.ExpiresAt = DateTime.Parse(responseData.data.token.expiresAt.date.ToString());
                        }
                        else
                        {
                            throw new Exception("Nem sikerült frissíteni a tokent.");
                        }
                    }
                    else
                    {
                        throw new Exception($"Hiba történt a token frissítésekor: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba történt a refreshTokenAsync függvényben: {ex.Message}");
            }
        }

        //Post, Delete metódusok kezelése
        public static async Task SendRequestAsync<T>(HttpMethod method, string endpoint, T data)
        {
            try
            {
                // Token érvényességének ellenőrzése
                if (Token.ExpiresAt <= DateTime.Now)
                {
                    await RefreshTokenAsync(); // Ha a token lejárt, frissítjük
                }


                string jsonData = JsonConvert.SerializeObject(data);
                Debug.WriteLine(jsonData);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Access_token);

                    var request = new HttpRequestMessage(method, $"{BaseUrl}{endpoint}")
                    {
                        Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
                    };

                    HttpResponseMessage response = await client.SendAsync(request);
                    Debug.WriteLine(response.StatusCode);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"HIBA: {errorMessage}");
                        throw new Exception($"Hiba a kérés során: {errorMessage}");
                    }
                    else
                    {

                        string result = await response.Content.ReadAsStringAsync();
                        // JSON válasz deszerializálása
                        var responseObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);

                        // Kiírjuk a deszerializált objektumot, hogy lássuk, mit tartalmaz
                        Debug.WriteLine($"Kapot válasz: {JsonConvert.SerializeObject(responseObject)}");

                        // Ellenőrzés, hogy a válaszban van-e success: false
                        if (responseObject.ContainsKey("success"))
                        {
                            bool success = Convert.ToBoolean(responseObject["success"]);
                            if (!success)
                            {
                                var error = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseObject["error"].ToString());
                                string errorMessage = error["message"].ToString();
                                MessageBox.Show($"HIBA: {errorMessage}");
                            }
                        }
                        else
                        {
                            // Ha a válasz nem tartalmazza a "success" kulcsot
                            MessageBox.Show("Nem található 'success' mező a válaszban!");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba történt a SendRequestAsync függvényben: {ex.Message}");
            }
        }
    }
}

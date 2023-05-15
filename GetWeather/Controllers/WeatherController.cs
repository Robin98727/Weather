using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GetWeather.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetWeather.Controllers
{
    public class WeatherController : ApiController
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey = "CWB-C3AF3EF3-C7D7-4566-B1D1-4961E0D295DB";

        public WeatherController()
        {
            httpClient = new HttpClient();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetWeather()
        {
            string apiUrl = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001"; // 替換為你想要擷取的資料API的URL


            // 建立HTTP請求並設定必要的標頭
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            request.Headers.Add("Authorization", apiKey);

            // 發送HTTP請求並取得回應
            HttpResponseMessage response = await httpClient.SendAsync(request);

            // 檢查回應狀態碼
            if (response.IsSuccessStatusCode)
            {
                // 讀取回應內容
                string json = await response.Content.ReadAsStringAsync();

                // 解析 JSON 字串
                JObject jsonObject = JObject.Parse(json);

                // 替換為你的資料庫連線字串
                string connectionString = "Server=MSI;Database=weather;User Id=sa;Password=1111;";


                for (int i = 0; i < 21; i++)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString)) 
                    {
                        connection.Open();

                        SqlCommand tablecount = new SqlCommand("SELECT Count(*) FROM Location", connection);

                        int rowCount = (int)tablecount.ExecuteScalar();

                        if (rowCount > 0)
                        {
                            string query = "UPDATE Location SET weatherElement = @weatherElement WHERE locationName = @locationName;";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@locationName", (string)jsonObject["records"]["location"][i]["locationName"]);
                                command.Parameters.AddWithValue("@weatherElement", (object)jsonObject["records"]["location"][i]["weatherElement"].ToString());

                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string query = "INSERT INTO Location (locationName, weatherElement) VALUES (@locationName, @weatherElement);";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@locationName", (string)jsonObject["records"]["location"][i]["locationName"]);
                                command.Parameters.AddWithValue("@weatherElement", (object)jsonObject["records"]["location"][i]["weatherElement"].ToString());

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }

                return Ok(json); // 回傳JSON回應
            }
            else
            {
                return BadRequest("Failed to retrieve weather data."); // 回傳錯誤回應
            }
        }
    }
}

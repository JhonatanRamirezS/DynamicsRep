using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace TranslatorService
{
    public class OprationalProgram
    {
        public static string jsonres;
        static string host = "https://api.cognitive.microsofttranslator.com";
        static string path = "/translate?api-version=3.0";
        //indicamos se traducira a ingles
        static string parmsfrom = "&from=en";
        //indicamos se traducira a español
        static string paramsto = "&to=es";
        //con los valores armamos la uir
        static string uri = host + path + parmsfrom + paramsto;
        //ponemos la llave generada desde Azure
        static string key = "0afe0845f93f4db88064b9bbeb43085b";

        async static Task<string> Translate(string _text)
        {
            System.Object[] body = new System.Object[] { new { Text = _text } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);
                string jsonresultante = result.ToString();
                int textpos = jsonresultante.IndexOf("text");
                jsonresultante = jsonresultante.Substring(textpos);
                textpos = jsonresultante.IndexOf('"');
                jsonresultante = jsonresultante.Substring(textpos + 1);
                textpos = jsonresultante.IndexOf('"');
                jsonresultante = jsonresultante.Substring(textpos+1);
                textpos = jsonresultante.IndexOf('"');
                jsonresultante = jsonresultante.Substring(0, textpos);
                jsonres = jsonresultante;

                return jsonres;
            }
        }
        public static string main(string _labeltext)
        {
            string textotra;
            ;
            Task<string> traduccion = Translate(_labeltext);
            traduccion.Wait();
            textotra = jsonres;
            return textotra;
        } 
    }
}

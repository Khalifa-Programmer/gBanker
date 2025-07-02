using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public class PnmApiClient : IDisposable
{
    private HttpClient client;
    private String host;
    private String secret;

    private Dictionary<String, String> parameters;

    public PnmApiClient(String host, String secret)
    {
        client = new HttpClient();
        client.BaseAddress = new Uri(host);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        this.host = host;
        this.secret = secret;
        parameters = new Dictionary<String, String>();
    }

    public HttpResponseMessage Execute(string action)
    {
        String url = string.Format("{0}?{1}", action, queryString());

        try
        {
            var response = client.GetAsync(url).Result;
           
            return response;
        }
        catch (IOException e)
        {
            return null;
        }
    }

    public void setParam(String param, String value)
    {
        parameters.Add(param, value);
    }


    public String getUrlString()
    {
        try
        {

            return String.Format("{0}?{1}", host, queryString());
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public String queryString()
    {
        Dictionary<String, String> signed = signedParameters();
        List<String> pairs = new List<String>();
        foreach (KeyValuePair<string, string> key in signed)
        {
            pairs.Add(key.Key + "=" + HttpUtility.UrlEncode(key.Value));
        }
        return String.Join("&", pairs.ToArray());
    }

    private static List<String> IGNORE_PARAMS = new List<string>() { "signature" };

    /**
     * Return a Map of all parameters as well as a signature for the items.
     */
    public Dictionary<String, String> signedParameters()
    {


        // Get the keySet, sort it.
        List<String> keys = new List<String>(parameters.Keys.ToArray());
        keys.Sort();

        StringBuilder str = new StringBuilder();
        foreach (String key in keys)
        {
            if (!IGNORE_PARAMS.Contains(key))
            {
                str.Append(key).Append(parameters[key]);
            }
        }
        str.Append(secret);
        String signature = "";
        using (MD5 md5Hash = MD5.Create())
        {
            signature = GetMd5Hash(md5Hash, str.ToString());
        }

        Dictionary<String, String> copy = new Dictionary<String, String>(parameters);
        copy.Add("signature", signature);
        return copy;
    }

    private string GetMd5Hash(MD5 md5Hash, string input)
    {

        // Convert the input string to a byte array and compute the hash. 
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes 
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data  
        // and format each one as a hexadecimal string. 
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string. 
        return sBuilder.ToString();
    }

    public void Dispose()
    {
        if (client != null)
            client.Dispose();
        client = null;
    }
}
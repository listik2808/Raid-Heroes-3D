using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text;
using System;

public class SaveData : MonoBehaviour
{
    private readonly string path = @"C:\note.txt";
    [SerializeField] private InputField _aInput;
    [SerializeField] private InputField _bInput;
    [SerializeField] private InputField _cInput;
    [SerializeField] private InputField _s1Input;
    [SerializeField] private InputField _s2Input;

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        var data = new TestData();
        data.a = int.TryParse(_aInput?.text, out int a) ? a : 0;
        data.b = int.TryParse(_bInput?.text, out int b) ? b : 0;
        data.c = int.TryParse(_cInput?.text, out int c) ? c : 0;
        data.s1 = _s1Input?.text;
        data.s2 = _s2Input?.text;

        var text = JsonConvert.SerializeObject(data);

        using (FileStream fstream = new FileStream(path, FileMode.Create))
        {
            byte[] buffer = Encoding.Default.GetBytes(text);
            fstream.Write(buffer, 0, buffer.Length);
        }
    }

    public void Load()
    {
        using (FileStream fstream = File.Open(path, FileMode.OpenOrCreate))
        {
            byte[] buffer = new byte[fstream.Length];
            fstream.Read(buffer, 0, buffer.Length);
            string text = Encoding.Default.GetString(buffer);
            var obj = JsonConvert.DeserializeObject<TestData>(text);
            Debug.Log(text);
        }
    }
}

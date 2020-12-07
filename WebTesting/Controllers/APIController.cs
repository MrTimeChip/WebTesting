using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace WebTesting.Controllers
{
    public class APIController : Controller
    {
        private bool _isAllocated;
        // GET
        public IActionResult ResponseTime(int v)
        {
            if(v == 2)
                Thread.Sleep(100);
            return new JsonResult(null);
        }
        
        [HttpPost]
        public IActionResult CPU(int v, [FromBody] string content)
        {
            var result = DeserializeSerialize(content);
            if (v != 2) return new JsonResult(null);
            for (var i = 0; i < 5; i++)
                DeserializeSerialize(content);

            return new JsonResult(result);
        }
        
        private static string DeserializeSerialize(string content)
        {
            var deserialized = JsonSerializer.Deserialize(content, typeof(Dummy));
            return JsonSerializer.Serialize(deserialized);
        }

        public IActionResult MemoryUsage(int v)
        {
            if (v != 2) return new JsonResult(null);
            for (var i = 0; i < 10000; i++)
            {
                var list = new List<int>();
                for (var j = 0; j < 10000; j++)
                    list.Add(j);
            }

            return new JsonResult(null);
        }

        public IActionResult MemoryLeak(int v)
        {
            if(v != 2) return new JsonResult(null);
            if (_isAllocated) return new JsonResult(null);
            Marshal.AllocHGlobal(1000);
            _isAllocated = true;
            return new JsonResult(null);
        }
    }
    
    public class Dummy
    {
        public int FirstInt { get; set; }
        public int SecondInt { get; set; }
        public byte[] Arr { get; set; }
        public string DummyName { get; set; }

        public Dummy()
        {
            FirstInt = 1;
            SecondInt = 2;
            Arr = new byte[] {2, 3, 3, 4, 5, 6, 1, 2};
        }
    }
}
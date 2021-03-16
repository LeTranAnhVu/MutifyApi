using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mutify.Helpers;
using Mutify.Models;
using NYoutubeDL;

namespace Mutify.Controllers
{
    [Route("api/youtube-dl")]
    public class YoutubeDownloadController : ControllerBase
    {
        protected readonly MutifyContext _context;
        protected readonly IMapper _mapper;

        public YoutubeDownloadController(MutifyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery(Name="url")] string url)
        {
            var savePath = "nhacs.mp3";
            Console.WriteLine("save path: " + savePath);

            var youtubeDl = new YoutubeDL(@"C:\Users\BrianLe\Downloads\youtube-dl");
            youtubeDl.Options.FilesystemOptions.Output = savePath;
            youtubeDl.Options.PostProcessingOptions.ExtractAudio = false;
            youtubeDl.VideoUrl = url;
            youtubeDl.PythonPath = @"C:\Python39\python.exe";

            youtubeDl.StandardOutputEvent += (sender, output) => Console.WriteLine(output);
            youtubeDl.StandardErrorEvent += (sender, errorOutput) => Console.WriteLine(errorOutput);


            await youtubeDl.DownloadAsync();

            return Ok(url);
        }
    }
}
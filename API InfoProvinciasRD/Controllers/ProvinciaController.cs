using API_InfoProvinciasRD.Models;
using API_InfoProvinciasRD.Models.Dto;
using API_InfoProvinciasRD.Repository.IConfiguration;
using API_InfoProvinciasRD.Repository.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Controllers
{
    [Authorize]
    [Route("api/Provincia")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiProvincia")] //SwagerEndPoint
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ProvinciaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;

        public ProvinciaController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Get all the Provincias
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProvinciaDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllProvincias()
        {
            var listaProvincias = await _unitOfWork.Provincia.GetAll();

            var provinciasDto = new List<ProvinciaDto>();

            foreach (var listaProv in listaProvincias)
            {
                provinciasDto.Add(_mapper.Map<ProvinciaDto>(listaProv));
            }

            return Ok(provinciasDto);
        }

        /// <summary>
        /// Get a Provincia by it's Id
        /// </summary>
        /// <param name="provinciaId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{provinciaId:int}", Name = "GetProvinciaById")]
        [ProducesResponseType(200, Type = typeof(ProvinciaDto))]  // El 'ProducesResponseType' es importante ponerlo
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetProvinciaById(int provinciaId)
        {
            var provincia = await _unitOfWork.Provincia.GetById(provinciaId);

            if (provincia == null)
            {
                return NotFound();
            }

            var provinciaDto = _mapper.Map<ProvinciaDto>(provincia);

            return Ok(provinciaDto);
        }


        /// <summary>
        /// Search for a Provincia by it's name
        /// </summary>
        /// <param name="nameProvincia"></param>
        /// <returns></returns>
        [AllowAnonymous]
        //REPARAR ERROR STATUS CODE 500
        [HttpGet("SearchProvincia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SearchProvincia(string nameProvincia)
        {
            try
            {
                var result = await _unitOfWork.Provincia.Search(nameProvincia);
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación");
            }
        }


        /// <summary>
        /// Create a Provincia
        /// </summary>
        /// <param name="provinciaCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ProvinciaCreateDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProvincia([FromForm] ProvinciaCreateDto provinciaCreateDto)
        {
            if (provinciaCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (await _unitOfWork.Provincia.Exist(provinciaCreateDto.Nombre))
            {
                ModelState.AddModelError("", $"La región {provinciaCreateDto.Nombre} ya existe.");
                return StatusCode(404, ModelState);
            }


            /******************************************************************************/
            /* Subida de archivos */
            var archivo = provinciaCreateDto.Foto;
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivo.Length > 0)
            {
                //Nueva imagen

                var nombreFoto = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"fotos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStream);
                }

                provinciaCreateDto.RutaImagen = @"\fotos\" + nombreFoto + extension;
            }
            /******************************************************************************/

            var provincia = _mapper.Map<Provincia>(provinciaCreateDto);

            if (!await _unitOfWork.Provincia.Add(provincia))
            {
                ModelState.AddModelError("", $"Algo salio mal agregando el resgistro {provincia.Nombre}.");
                return StatusCode(500, ModelState);
            }

            await _unitOfWork.CompleteAsync();

            return CreatedAtRoute("GetProvinciaById", new { provinciaId = provincia.Id }, provincia);
        }

        /// <summary>
        /// Update Provincia
        /// </summary>
        /// <param name="provinciaId"></param>
        /// <param name="provinciaUpdateDto"></param>
        /// <returns></returns>
        [HttpPatch("{provinciaId:int}", Name = "UpdateProvincia")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProvincia(int provinciaId, [FromForm] ProvinciaUpdateDto provinciaUpdateDto)
        {
            if (provinciaUpdateDto == null || provinciaId != provinciaUpdateDto.Id)
            {
                return BadRequest(ModelState);
            }


            /******************************************************************************/
            /* Subida de archivos */
            var archivo = provinciaUpdateDto.Foto;
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivo.Length > 0)
            {
                //Actualizar imagen
                var nombreFoto = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"fotos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStream);
                }

                provinciaUpdateDto.RutaImagen = @"\fotos\" + nombreFoto + extension;
            }
            /******************************************************************************/



            var provincia = _mapper.Map<Provincia>(provinciaUpdateDto);

            if (!await _unitOfWork.Provincia.Update(provincia))
            {
                ModelState.AddModelError("", $"Algo salio mal agregando el resgistro {provincia.Nombre}.");
                return StatusCode(500, ModelState);
            }

            await _unitOfWork.CompleteAsync();

            return Content("Se actualizo correctamente");

        }

        /// <summary>
        /// Delete an existing Provincia
        /// </summary>
        /// <param name="provinciaId"></param>
        /// <returns></returns>
        [HttpDelete("{provinciaId:int}", Name = "DeleteProvincia")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProvincia(int provinciaId)
        {
            if (!await _unitOfWork.Provincia.Exist(provinciaId))
            {
                return NotFound();
            }

            var provincia = await _unitOfWork.Provincia.GetById(provinciaId);

            if (!await _unitOfWork.Provincia.Delete(provincia))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {provincia.Nombre}");
                return StatusCode(500, ModelState);
            }

            await _unitOfWork.CompleteAsync();

            return Content($"Se elimino el registro {provincia.Nombre}");
        }


    }
}

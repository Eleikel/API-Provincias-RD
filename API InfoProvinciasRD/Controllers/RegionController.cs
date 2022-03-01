using API_InfoProvinciasRD.Models;
using API_InfoProvinciasRD.Models.Dto;
using API_InfoProvinciasRD.Repository.IConfiguration;
using API_InfoProvinciasRD.Repository.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Controllers
{
    [Authorize]
    [Route("api/Region")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiRegion")] //SwagerEndPoint
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class RegionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public RegionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// Get all the Regions
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegionDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRegions()
        {
            var listaRegiones = await _unitOfWork.Region.GetAll();

            var listaRegionesDto = new List<RegionDto>();

            foreach (var lista in listaRegiones)
            {
                listaRegionesDto.Add(_mapper.Map<RegionDto>(lista));
            }

            return Ok(listaRegionesDto);
        }

        /// <summary>
        /// Get Region by it's Id
        /// </summary>
        /// <param name="RegionId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{RegionId:int}", Name = "GetRegion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetRegion(int RegionId)
        {
            var itemRegion = await _unitOfWork.Region.GetById(RegionId);

            if (itemRegion == null)
            {
                return NotFound();
            }

            var itemRegionDto = _mapper.Map<RegionDto>(itemRegion);
            return Ok(itemRegionDto);
        }

        /// <summary>
        /// Create a Region
        /// </summary>
        /// <param name="regionDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRegion([FromBody] RegionDto regionDto)
        {

            if (regionDto == null)
            {
                return BadRequest(ModelState);
            }

            if (await _unitOfWork.Region.Exist(regionDto.NombreRegion))
            {
                ModelState.AddModelError("", $"La región {regionDto.NombreRegion} ya existe.");
                return StatusCode(404, ModelState);
            }

            var regionMap = _mapper.Map<Region>(regionDto);

            if (!await _unitOfWork.Region.Add(regionMap))
            {
                ModelState.AddModelError("", $"Algo salio mal agregando el resgistro {regionMap.NombreRegion}.");
                return StatusCode(500, ModelState);
            }

            await _unitOfWork.CompleteAsync();

            return CreatedAtRoute("GetRegion", new { regionID = regionMap.Id }, regionMap);
        }

        /// <summary>
        /// Update Region
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="regionDto"></param>
        /// <returns></returns>
        [HttpPatch("{regionId:int}", Name = "UpdateRegion")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRegion(int regionId, [FromBody] RegionDto regionDto)
        {
            if (regionDto == null || regionId != regionDto.Id)
            {
                return BadRequest(ModelState);
            }

            var regionMap = _mapper.Map<Region>(regionDto);

            if (!await _unitOfWork.Region.Update(regionMap))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el resgistro {regionMap.NombreRegion}");
                return StatusCode(500, ModelState);
            }

            await _unitOfWork.CompleteAsync();

            return Content("Se actualizo correctamente");
        }

        /// <summary>
        /// Delete Region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        [HttpDelete("{regionId:int}", Name = "DeleteRegion")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> DeleteRegion(int regionId)
        {
            if (!await _unitOfWork.Region.Exist(regionId))
            {
                return NotFound();
            }

            var region = await _unitOfWork.Region.GetById(regionId);

            if (!await _unitOfWork.Region.Delete(region))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {region.NombreRegion}");
                return StatusCode(500, ModelState);
            }

            await _unitOfWork.CompleteAsync();

            return Content("Registro eliminado sastifactoriamente.");
        }

    }
}

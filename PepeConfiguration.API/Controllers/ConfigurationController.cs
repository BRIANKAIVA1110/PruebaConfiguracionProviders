using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PepeConfiguration.API.DTOs;
using PepeConfiguration.API.Hubs;
using PepeConfiguration.API.Infraestructura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace PepeConfiguration.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ConfigurationController: ControllerBase
    {
        private readonly IHubContext<PepeConfigurationHub> _pepeConfigurationHub;
        private readonly ApplicationContext _applicationContext;
        public ConfigurationController(IHubContext<PepeConfigurationHub> pepeConfigurationHub, ApplicationContext applicationContext)
        {
            _pepeConfigurationHub = pepeConfigurationHub;
            _applicationContext = applicationContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ConfiguracionDTO>))]
        public IActionResult Get()
        {
            var entity = _applicationContext.Configuraciones.ToList();

            return Ok(entity);//esta retornando la entidad(clase poco) del modelo de datos.. deberia enviar un dto/viewModel
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConfiguracionDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromRoute] int Id) 
        {
            var entity = _applicationContext.Configuraciones.FirstOrDefault(x=> x.Id == Id);
            if (entity is null)
                return NotFound();

            return Ok(entity);//esta retornando la entidad(clase poco) del modelo de datos.. deberia enviar un dto/viewModel
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromRoute] int Id)
        {
            var entity = _applicationContext.Configuraciones.FirstOrDefault(x => x.Id == Id);
            if (entity is null)
                return NotFound();

            return Ok(entity);//esta retornando la entidad(clase poco) del modelo de datos.. deberia enviar un dto/viewModel
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody]ConfiguracionDTO configuracionDTO)
        {
            var entity = _applicationContext.Configuraciones.FirstOrDefault(x => x.Section == configuracionDTO.Section &&
                            x.Key == configuracionDTO.Key);

            entity.Value = configuracionDTO.Value;
            _applicationContext.Update(entity);

            int result = _applicationContext.SaveChanges();
            if (result > 0)
            {
                //ENVIA MSG A CLIENTES DE GENERADOS DESDE "FRAMEWORK" CONFIGURACION.
                //EJECUTA EL METODO "pepeTopic" EN LOS CLIENTES DEL FRAMEWORK-> CONFIGURACIONCENTRALIZADA
                await _pepeConfigurationHub.Clients.All.SendAsync("pepeTopic", "modificaion en ........");
                return Ok();
            }
            else 
            {
                return NoContent();
            }
        }


        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Patch([FromBody] JsonPatchDocument<ConfiguracionDTO> patchDoc,[FromRoute] int Id)
        {
            var entity = _applicationContext.Configuraciones.FirstOrDefault(x => x.Id == Id);

            if (entity is null)
                return NotFound();
            var configuracionDto = new ConfiguracionDTO();
            patchDoc.ApplyTo(configuracionDto);

            //las siguientes lineas deben estan mas controladas por temas de reemplazo de algo con valor por nul ya que no esta en el patch
            entity.Key = configuracionDto.Key;
            entity.Section = configuracionDto.Section;
            entity.Value = configuracionDto.Value;
            //modificacion parcial de una configuracion....

            return Ok(entity);//esta retornando la entidad(clase poco) del modelo de datos.. deberia enviar un dto/viewModel
        }
    }
}

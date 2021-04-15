using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PepeConfiguration.API.DTOs;
using PepeConfiguration.API.Hubs;
using PepeConfiguration.API.Infraestructura;
using PepeConfiguration.API.Infraestructura.Entities;
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
    public class ConfigurationController : ControllerBase
    {
        private readonly IHubContext<PepeConfigurationHub> _pepeConfigurationHub;
        private readonly ApplicationContext _applicationContext;
        public ConfigurationController(IHubContext<PepeConfigurationHub> pepeConfigurationHub, ApplicationContext applicationContext)
        {
            _pepeConfigurationHub = pepeConfigurationHub ?? throw new ArgumentNullException(nameof(pepeConfigurationHub));

            //este deberia estar con una implementacion de patron repositorio y este interactuar con el contexto
            //el controller solo debe enteractuar con una "capa" de servicios
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ConfiguracionDTO>))]
        public IActionResult Get()
        {
            // solo debe haber logica de applicaion y no de negocio en los controllers.
            //esta linea deberia estar en la capa de negocio con logica de obtencion de configuracion segun el mismo
            var entity = _applicationContext.Configuraciones.ToList();

            return Ok(entity);//esta retornando la entidad(clase poco) del modelo de datos.. deberia enviar un dto/viewModel
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConfiguracionDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromRoute] int Id) 
        {
            //esta linea deberia estar en la capa de negocio con logica de obtencion de configuracion segun el mismo
            var entity = _applicationContext.Configuraciones.FirstOrDefault(x=> x.Id == Id);
            if (entity is null)
                return NotFound();

            return Ok(entity);//esta retornando la entidad(clase poco) del modelo de datos.. deberia enviar un dto/viewModel
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] ConfiguracionDTO configuracionDTO)
        {

            //estas linead deberian estar en la capa de negocio
            var configuracion = new Configuracion();
            configuracion.Section = configuracionDTO.Section;
            configuracion.Key = configuracionDTO.Key;
            configuracion.Value = configuracionDTO.Value;
            configuracion.Cluster = configuracionDTO.Cluster;
            configuracion.ApplicationName = configuracionDTO.ApplicationName;

            _applicationContext.Configuraciones.Add(configuracion);
            int result = _applicationContext.SaveChanges();
            if (result > 0)
                return CreatedAtAction("Get", new { Id= configuracion.Id});//esta retornando la entidad(clase poco) del modelo de datos.. deberia enviar un dto/viewModel


            return BadRequest();//esto esta mal.. deberia estrar en un try...
        }


        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody]ConfiguracionDTO configuracionDTO, [FromRoute] int Id)
        {
            //esta linea deberia estar en la capa de negocio con logica de obtencion de configuracion segun el mismo
            var entity = _applicationContext.Configuraciones.FirstOrDefault(x => x.Id == Id);

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


        [HttpPatch("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Patch([FromBody] JsonPatchDocument<ConfiguracionDTO> patchDoc,[FromRoute] int Id)
        {
            //esta linea deberia estar en la capa de negocio con logica de obtencion de configuracion segun el mismo
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

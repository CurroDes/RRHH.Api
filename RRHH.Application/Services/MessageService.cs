using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace RRHH.Application.Services
{
    public class MessageService : IMessageService
    {

        private readonly HttpClient _httpClient;
        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result> MessageApi(MessageDTO m)
        {
            Result result = new Result();

            try
            {
                var url = "https://localhost:7136/api/Message/";

                // Serializamps el objeto a JSON
                var content = new StringContent(JsonSerializer.Serialize(m), Encoding.UTF8, "application/json");

                // Realizamos la solicitud POST
                var response = await _httpClient.PostAsync(url, content);

                // Verificamos si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Si es exitosa, lee el contenido como un string
                    var responseContent = await response.Content.ReadAsStringAsync();
                    result.Text = responseContent; // Asignamos el contenido a result.Text
                }
                else
                {
                    // Si no es exitosa, asignamos un mensaje de error
                    result.Error = "Ha habido un error al intentar comunicarse con el empleado respecto a su solicitud";
                    result.IsSuccess = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Si ocurre alguna excepción, asignamos el mensaje de error
                result.Error = $"Error en la comunicación: {ex.Message}";
            }

            return result;
        }
    }
}

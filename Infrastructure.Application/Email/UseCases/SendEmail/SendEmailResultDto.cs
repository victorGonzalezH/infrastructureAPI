﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.API.Application.Email.UseCases.SendEmail
{


    public class SendEmailResultDto
    {
        /// <summary>
        /// Identificador de la operacion de envio resultante
        /// </summary>
        public string Id { get; private set; }


        public SendEmailResultDto(string id)
        {
            Id = id;
        }

    }
}
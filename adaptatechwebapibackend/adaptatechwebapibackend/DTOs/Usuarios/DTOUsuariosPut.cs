﻿using System;
namespace adaptatechwebapibackend.DTOs.Usuarios
    {
    public class DTOUsuariosPut
        {
        public string Email { get; set; }
        public string PasswordActual { get; set; }
        public string PasswordNuevo { get; set; }

        }
    }

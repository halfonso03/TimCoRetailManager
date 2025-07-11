﻿using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace TRMDataManager2.App_Start
{
    public class AuthTokenOperation : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths.Add("/token", new PathItem
            {
                post = new Operation
                {
                    tags = new List<string> { "Auth" },
                    consumes = new List<string>
                    {
                        "application/x-www-form-urlencoded"
                    },
                    parameters  = new List<Parameter>
                    {
                        new Parameter
                        {
                            type = "string",
                            name = "grant_type",
                            required = true,
                            @in = "formData",
                            @default = "password"
                        },
                         new Parameter
                        {
                            type = "string",
                            name = "username",
                            required = true,
                            @in = "formData",
                            @default = "hector.alfonso@yahoo.com"
                        },
                          new Parameter
                            {
                            type = "string",
                            name = "password",
                            required = true,
                            @in = "formData",
                            @default = "Password#1"
                          }
                    }
                }
            });
        }
    }
}
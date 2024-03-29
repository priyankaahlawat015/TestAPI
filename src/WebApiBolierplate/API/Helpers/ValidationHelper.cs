﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Helpers;

public class ValidationHelper
{
    private readonly WebAPIHelper webAPIHelper;

    public ValidationHelper()
    {
        webAPIHelper = new WebAPIHelper();
    }

    /// <summary>
    /// Obtains all model validation errors and combaines them into a single CSV error text
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public BadRequestObjectResult GetDataValidationError(ModelStateDictionary model)
    {
        var errorText = string.Empty;
        foreach (var value in model.Values)
        {
            for (var i = 0; i < value.Errors.Count; i++)
            {
                errorText = errorText + ", " + value.Errors[i].ErrorMessage;
            }
        }
        errorText = errorText[1..].Trim();
        return webAPIHelper.CreateBadRequest(errorText);
    }
}

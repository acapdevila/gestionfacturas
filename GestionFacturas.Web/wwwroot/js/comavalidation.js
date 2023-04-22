 
          //Per la validació dels números decimals (coma per punts)
          $.validator.methods.number = function (value, element) {
              value = floatValue(value);
              return this.optional(element) || !isNaN(value);
          }
          $.validator.methods.range = function (value, element, param) {
              value = floatValue(value);
              return this.optional(element) || (value >= param[0] && value <= param[1]);
          }

          function floatValue(value) {
              return parseFloat(value.replace(",", "."));
          }  

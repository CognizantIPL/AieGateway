var Gateway = Gateway || {};
Gateway.Api = (function ($) {
    return {
        initialize: function () {
            var temperature = Gateway.Common.getParameterByName("temperature");
            var wetdry = Gateway.Common.getParameterByName("wetdry");

            if (temperature > 64) {
                Gateway.Azure.postToAzure(temperature, wetdry);
            }
        }
    };

}(jQuery));



Gateway.Azure = (function ($) {
    return {
        getTable: function (tableName) {
            var azureClient = new WindowsAzure.MobileServiceClient('https://aiemobileservice.azure-mobile.net/', 'NYuUVUztAwEXJQZxOFbppximTExpoh26');
            return azureClient.getTable(tableName);
        },

        postToAzure: function (temp, wetdry) {
            var message = (wetdry === "wet") ? "ALERT: Moisture detected" : "ALERT: Temperature threshold exceeded";

            var registeredDevices = Gateway.Azure.getTable("app_device");

            registeredDevices.read().done(function (device) {
                for (var i = 0; i < device.length; i++) {
                    if (device[i].channel)
                        Gateway.Azure.post({ temperature: temp, msg: message, channel: device[i].channel });
                }

            }, function (err) {
                alert("Error: " + err);
            });
        },

        post: function (messageData) {
            var headers = {
                'X-ZUMO-APPLICATION': 'NYuUVUztAwEXJQZxOFbppximTExpoh26'
            };
            //{ insuredId: "1", equipmentId: "3", temperature: 51, incidentTimestamp: "2013-10-08T12:11:00.000Z", activeIndicator: false }

            $.ajax({
                url: "https://aiemobileservice.azure-mobile.net/tables/equipment_incident",
                type: "POST",
                async: false,
                headers: headers,
                data: messageData,
                dataType: "json",
                success: function (result) {
                    Gateway.Common.log("Azure POST succesful for channel:" + result.channel);
                },
                error: function (result) {
                    Gateway.Common.log(("Error:" + result));
                }
            });
        }
    };

}(jQuery));



Gateway.Common = (function ($) {
    return {
        getParameterByName: function (name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        },
        log: function (message) {
            $('#errorlog').append($('<li>').text(message));
        }
    };

}(jQuery));
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace Hoot
{
    internal class Calendar
    {
       public async void Connect()
        {
            var service = new CalendarService(new BaseClientService.Initializer
            {
                ApplicationName = "Calendar",
                ApiKey = "[YOUR_API_KEY_HERE]",
            });

            var result = await service.Calendars.GetAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace RESTAPI
{
    public interface IPeopleAPI
    {
        IActionResult Get(string sort);
        IActionResult Post(string line);
    }
}

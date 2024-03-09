using Microsoft.AspNetCore.Mvc;
using yard.application.Services.Interface;

namespace yard.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : BaseApiController
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        [HttpGet("available-rooms/{roomTypeId}")]
        public async Task<IActionResult> GetAvailableRooms(int hotelId, int roomTypeId)
        {
               var availableRooms = await _roomService.GetAvailableRooms(hotelId, roomTypeId);
                return Response(availableRooms);
        }

        [HttpGet("get-room-type/{hotelId}/{roomtypeId}")]
        public async Task<IActionResult> GetRoomsInRoomType(int hotelId, int roomtypeId)
        {
            var result = await _roomService.GetRoomsInRoomType(hotelId, roomtypeId);
            return Response(result);
        }
    }
}


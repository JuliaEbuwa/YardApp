using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yard.domain.Models;
using yard.domain.ViewModels;

namespace yard.application.Services.Interface
{
    public interface IRoomService
    {
        Task<ApiResponse> GetAvailableRooms(int hotelId, int roomTypeId);
        Task<ApiResponse> GetRoomsInRoomType(int hotelId, int roomtypeId);
    }
}

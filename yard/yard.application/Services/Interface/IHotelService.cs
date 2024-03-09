using Azure;
using yard.domain.Models;
using yard.domain.ViewModels;

namespace yard.application.Services.Interface
{
	public interface IHotelService
	{
		Task<List<HotelVM>> GetHotelsAsync();

		Task<HotelVM> GetHotelAsync(int id);

		Task<List<HotelVM>> GetHotelsAsync(string name, string state, string searchString);

		Task<HotelVM> SaveHotelAsync(HotelVM hotelVM);

		Task<bool> HotelExistAsync(int hotelId);

		Task<List<RoomTypeVM>> GetRoomsInHotelAsync(int hotelId);

        Task<object> GetHotelWithRoomsAsync(int hotelId, bool includeRoomType);
        Task<bool> DelistHotelAsync(int hotelId);

        Task<ApiResponse> AddHotel(HotelVM addhotelVM);

        Task<ApiResponse> UpdatedHostelAsync(HotelVM hotel);
    }
}
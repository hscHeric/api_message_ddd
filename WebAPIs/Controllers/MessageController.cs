using AutoMapper;
using Domain.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIs.Models;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _IMapper;
        private readonly IMessage _IMessage;

        public MessageController(IMapper iMapper, IMessage iMessage)
        {
            _IMapper = iMapper;
            _IMessage = iMessage;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/AddMessage")]
        public async Task<List<Notifies>> Add(MessageViewModel message)
        {
            message.UserId = await ReturnUserLogged();
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.Add(messageMap);

            return messageMap.Notifications;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/UpdateMessage")]
        public async Task<List<Notifies>> Update(MessageViewModel message)
        {
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.Update(messageMap);

            return messageMap.Notifications;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/DeleteMessage")]
        public async Task<List<Notifies>> Delete(MessageViewModel message)
        {
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.Delete(messageMap);

            return messageMap.Notifications;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/GetEntityById")]
        public async Task<MessageViewModel> GetEntityById(Message message)
        {
            message = await _IMessage.GetEntityById(message.Id);
            var messageMap = _IMapper.Map<MessageViewModel>(message);

            return messageMap;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ListMessage")]
        public async Task<List<MessageViewModel>> List()
        {
            var message = await _IMessage.List();
            var messageMap = _IMapper.Map<List<MessageViewModel>>(message);

            return messageMap;
        }

        private async Task<string> ReturnUserLogged()
        {
            if (User != null)
            {
                var idUser = User.FindFirst("idUser");
                return idUser.Value;
            }
            return string.Empty;
        }
    }
}

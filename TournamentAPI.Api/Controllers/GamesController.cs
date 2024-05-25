using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Dto;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
        {
            try
            {
                var games = await _unitOfWork.Games.GetAllAsync();
                var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
                return Ok(gameDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            try
            {
                var game = await _unitOfWork.Games.GetAsync(id);
                if (game == null)
                {
                    return NotFound();
                }
                var gameDto = _mapper.Map<GameDto>(game);
                return Ok(gameDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            var existingGame = await _unitOfWork.Games.GetAsync(id);
            if (existingGame == null)
            {
                return NotFound();
            }
            _mapper.Map(gameDto, existingGame);
            await _unitOfWork.Games.UpdateAsync(existingGame);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
        {
            var game = _mapper.Map<Game>(gameDto);
            await _unitOfWork.Games.AddAsync(game);
            await _unitOfWork.CompleteAsync();
            var createdGameDto = _mapper.Map<GameDto>(game);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, createdGameDto);
        }

        [HttpPatch("{GameId}")]
        public async Task<IActionResult> PatchGame(int gameId, JsonPatchDocument<GameDto> patchDocument)
        {
            var game = await _unitOfWork.Games.GetAsync(gameId);
            if (game == null)
            {
                return NotFound();
            }

            var gameDto = _mapper.Map<GameDto>(game);
            patchDocument.ApplyTo(gameDto, ModelState);

            if (!TryValidateModel(gameDto))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(gameDto, game);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }


        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _unitOfWork.Games.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            await _unitOfWork.Games.RemoveAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> GameExists(int id)
        {
            return await _unitOfWork.Games.AnyAsync(id);
        }
    }

}



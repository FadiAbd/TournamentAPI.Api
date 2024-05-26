using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using AutoMapper;
using TournamentAPI.Core.Dto;
using Azure;
using Microsoft.AspNetCore.JsonPatch;

namespace TournamentAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments()
        {
            try
            {
                var tournaments = await _unitOfWork.Tournaments.GetAllAsync();
                var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
                return Ok(tournamentDtos);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error");
            }

        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournament(int id)
        {
            try
            {
                var tournament = await _unitOfWork.Tournaments.GetAsync(id);
                if (tournament == null)
                {
                    return NotFound();
                }
                var tournamentDto = _mapper.Map<TournamentDto>(tournament);
                return Ok(tournamentDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        // PUT: api/Tournaments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentDto tournamentDto)
        {
            var existingTournament = await _unitOfWork.Tournaments.GetAsync(id);
            if (existingTournament == null)
            {
                return NotFound();
            }

            // Map properties from DTO to the existing entity
            _mapper.Map(tournamentDto, existingTournament);

            await _unitOfWork.Tournaments.UpdateAsync(existingTournament);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TournamentExists(id))
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

        // POST: api/Tournaments
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournament(TournamentDto tournamentDto)
        {
            var tournament = _mapper.Map<Tournament>(tournamentDto);
            await _unitOfWork.Tournaments.AddAsync(tournament);
            await _unitOfWork.CompleteAsync();

            var createdTournamentDto = _mapper.Map<TournamentDto>(tournament);
            return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, createdTournamentDto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTournament(int id, JsonPatchDocument<TournamentDto> patchDocument)
        {
            try
            {
                var tournament = await _unitOfWork.Tournaments.GetAsync(id);
                if (tournament == null)
                {
                    return NotFound();
                }

                var tournamentDto = _mapper.Map<TournamentDto>(tournament);
                patchDocument.ApplyTo(tournamentDto, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _mapper.Map(tournamentDto, tournament);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _unitOfWork.Tournaments.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            await _unitOfWork.Tournaments.RemoveAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> TournamentExists(int id)
        {
            return await _unitOfWork.Tournaments.AnyAsync(id);
        }
    }

}


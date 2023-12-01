﻿namespace TeamFinder.Models.DTO;

public class CreateActivityRequestDto
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool OpenRegistration { get; set; }
    public int MaxParticipant { get; set; }
    public string UrlHandle { get; set; }
    // Here will be the user that created the Activity
    public string CreatedBy { get; set; }
}
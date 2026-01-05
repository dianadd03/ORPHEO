# ORPHEO

---

**ORPHEO** is a collaborative music platform that transforms music listening into a shared, social experience.  
The application combines real-time interaction between users with AI-assisted tools for artists, focusing on emotion-aware music organization and remote collaboration.

---

## Project Idea

ORPHEO builds a social ecosystem around music, where users can:

- Listen to music together, in real time
- Interact socially around songs and playlists
- Support artists with intelligent tools for music description

The platform emphasizes interaction, collaboration, and emotional context, rather than passive music consumption.

---

## Session Rooms & User Codes

- Every user has a unique profile code
- Session rooms can be created based on these codes
- Friends can easily connect without exposing personal data
- Rooms support real-time interaction via SignalR

---

## Real-Time Collaboration (SignalR)

SignalR is a core component of ORPHEO and enables:

- Session rooms for shared listening
- Synchronous interaction between participants
- Live updates inside sessions
- User presence awareness

Users can create session rooms and invite friends using unique profile codes available in each user’s profile.  
This allows easy and secure collaboration without exposing personal information.

---

## Remote Listening (ngrok)

During development and testing, ngrok was used to expose the local application to the internet.  
This allowed users to participate in real-time listening sessions from different locations, validating the remote collaboration features built with SignalR.

Ngrok was used strictly as a development and demonstration tool.

---

## AI Companion – Lyrics Emotion Tagging

The AI Companion is designed to assist artists by analyzing song lyrics and identifying emotional content.

Its purpose is to:

- Analyze lyrics
- Detect emotions and moods
- Automatically generate emotion-based tags for songs

These tags help artists better describe their music and improve categorization, while final creative control remains with the artist.

---

## Music Interaction & Social Features

Each user can:

- Like songs
- Comment on tracks
- Create playlists:
  - Public playlists, visible to other users
  - Private playlists, for personal use
- Manage personal music collections

These features encourage engagement, discovery, and social interaction around music.

---

## Users & Roles

ORPHEO supports multiple user roles:

- User – listens to music, interacts socially, creates playlists and session rooms
- Artist – uploads music and uses AI-assisted emotion tagging
- Admin – manages users and role requests

Role upgrades are handled through an approval workflow.

---

## Technical Overview (Brief)

- ASP.NET Core (Razor Pages + MVC)
- SignalR for real-time communication
- ASP.NET Core Identity for authentication and roles
- Entity Framework Core for data persistence

---



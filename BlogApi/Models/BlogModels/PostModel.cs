﻿using BlogApi.Entities;

namespace BlogApi.Models.BlogModels;

public class PostModel
{
    public Guid PostId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } 
    public DateTime? UpdatedDate { get; set; }
    public bool IsLiked { get; set; }
    public int LikeCount { get; set; }
    public bool IsSaved { get; set; }

    public Guid BlogId { get; set; }
    public  List<Like_Saved_Model> Likes { get; set; }
    public  List<Like_Saved_Model> SavedPosts { get; set; }
    public List<CommentModel> Comments { get; set; }
}
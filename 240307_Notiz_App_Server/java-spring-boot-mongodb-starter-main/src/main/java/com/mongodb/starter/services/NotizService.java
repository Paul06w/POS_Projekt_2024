package com.mongodb.starter.services;


import com.mongodb.starter.dtos.NotizDTO;

import java.util.List;

public interface NotizService {

    NotizDTO save(NotizDTO NotizDTO);

    List<NotizDTO> saveAll(List<NotizDTO> notizEntities);

    List<NotizDTO> findAll();

    List<NotizDTO> findAll(List<String> ids);

    NotizDTO findOne(String id);

    long count();

    long delete(String id);

    long delete(List<String> ids);

    long deleteAll();

    NotizDTO update(NotizDTO NotizDTO);

    long update(List<NotizDTO> notizEntities);


}
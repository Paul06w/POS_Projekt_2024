package com.mongodb.starter.repositories;

import com.mongodb.starter.models.NotizEntity;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface NotizRepository {

    NotizEntity save(NotizEntity notizEntity);

    List<NotizEntity> saveAll(List<NotizEntity> notizEntities);

    List<NotizEntity> findAll();

    List<NotizEntity> findAll(List<String> ids);

    NotizEntity findOne(String id);

    long count();

    long delete(String id);

    long delete(List<String> ids);

    long deleteAll();

    NotizEntity update(NotizEntity notizEntity);

    long update(List<NotizEntity> notizEntities);

}

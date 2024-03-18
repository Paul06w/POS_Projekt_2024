package com.mongodb.starter.services;

import com.mongodb.starter.dtos.NotizDTO;
import com.mongodb.starter.repositories.NotizRepository;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class NotizServiceImpl implements NotizService {

    private final NotizRepository notizRepository;

    public NotizServiceImpl(NotizRepository notizRepository) {
        this.notizRepository = notizRepository;
    }

    @Override
    public NotizDTO save(NotizDTO NotizDTO) {
        return new NotizDTO(notizRepository.save(NotizDTO.toNotizEntity()));
    }

    @Override
    public List<NotizDTO> saveAll(List<NotizDTO> notizEntities) {
        return notizEntities.stream()
                .map(NotizDTO::toNotizEntity)
                .peek(notizRepository::save)
                .map(NotizDTO::new)
                .toList();
    }

    @Override
    public List<NotizDTO> findAll() {
        return notizRepository.findAll().stream().map(NotizDTO::new).toList();
    }

    @Override
    public List<NotizDTO> findAll(List<String> ids) {
        return notizRepository.findAll(ids).stream().map(NotizDTO::new).toList();
    }

    @Override
    public NotizDTO findOne(String id) {
        return new NotizDTO(notizRepository.findOne(id));
    }

    @Override
    public long count() {
        return notizRepository.count();
    }

    @Override
    public long delete(String id) {
        return notizRepository.delete(id);
    }

    @Override
    public long delete(List<String> ids) {
        return notizRepository.delete(ids);
    }

    @Override
    public long deleteAll() {
        return notizRepository.deleteAll();
    }

    @Override
    public NotizDTO update(NotizDTO NotizDTO) {
        return new NotizDTO(notizRepository.update(NotizDTO.toNotizEntity()));
    }

    @Override
    public long update(List<NotizDTO> notizEntities) {
        return notizRepository.update(notizEntities.stream().map(NotizDTO::toNotizEntity).toList());
    }
}
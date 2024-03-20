package com.mongodb.starter.controllers;

import com.mongodb.starter.dtos.NotizDTO;
import com.mongodb.starter.services.NotizService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api")
public class NotizController {

    private final static Logger LOGGER = LoggerFactory.getLogger(NotizController.class);
    private final NotizService notizService;

    public NotizController(NotizService notizService) {

        this.notizService = notizService;
    }

    @PostMapping("notiz")
    @ResponseStatus(HttpStatus.CREATED)
    public NotizDTO postNotiz(@RequestBody NotizDTO NotizDTO) {
        return notizService.save(NotizDTO);
    }

    @PostMapping("notizen")
    @ResponseStatus(HttpStatus.CREATED)
    public List<NotizDTO> postNotizen(@RequestBody List<NotizDTO> notizEntities) {
        return notizService.saveAll(notizEntities);
    }

    @GetMapping("notizen")
    public List<NotizDTO> getNotizen() {
        return notizService.findAll();
    }

    @GetMapping("notiz/{id}")
    public ResponseEntity<NotizDTO> getNotiz(@PathVariable String id) {
        NotizDTO NotizDTO = notizService.findOne(id);
        if (NotizDTO == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(NotizDTO);
    }

    @GetMapping("notizen/{ids}")
    public List<NotizDTO> getNotizen(@PathVariable String ids) {
        List<String> listIds = List.of(ids.split(","));
        return notizService.findAll(listIds);
    }

    @GetMapping("notizen/count")
    public Long getCount() {
        return notizService.count();
    }

    @DeleteMapping("notiz/{id}")
    public Long deleteNotiz(@PathVariable String id) {
        return notizService.delete(id);
    }

    @DeleteMapping("notizen/{ids}")
    public Long deleteNotizen(@PathVariable String ids) {
        List<String> listIds = List.of(ids.split(","));
        return notizService.delete(listIds);
    }

    @DeleteMapping("notizen")
    public Long deleteNotizen() {
        return notizService.deleteAll();
    }

    @PutMapping("notiz")
    public NotizDTO putNotiz(@RequestBody NotizDTO NotizDTO) {
        return notizService.update(NotizDTO);
    }

    @PutMapping("notizen")
    public Long putNotiz(@RequestBody List<NotizDTO> notizEntities) {
        return notizService.update(notizEntities);
    }

    /*@GetMapping("cars/averageAge")
    public Double averageAge() {
        return carService.getAverageAge();
    }
     */

    @ExceptionHandler(RuntimeException.class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    public final Exception handleAllExceptions(RuntimeException e) {
        LOGGER.error("Internal server error.", e);
        return e;
    }
}
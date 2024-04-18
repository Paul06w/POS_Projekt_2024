package com.mongodb.starter.controllers;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

@Controller
@RequestMapping("app")
public class WebController {

    @GetMapping("/")
    public String index2() {
        return "index2";
    }

}
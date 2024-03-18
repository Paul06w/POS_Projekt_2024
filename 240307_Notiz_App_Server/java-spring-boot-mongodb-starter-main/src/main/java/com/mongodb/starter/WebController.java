package com.mongodb.starter;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

@Controller
public class WebController {

    @GetMapping("/webb")
    public String hello(Model model) {
        model.addAttribute("message", "Hello, World!");
        return "index"; // Dies wird auf die HTML-Vorlage "index.html" verweisen
    }

    @GetMapping("/drivemenu")
    public String drivemenu(Model model) {
        model.addAttribute("message", "Hello, World!");
        return "drivemenu"; // Dies wird auf die HTML-Vorlage "drivemenu.html" verweisen
    }

    @PostMapping("/sendCommand")
    public String sendCommand(@RequestParam String command) {
        try {
            DatagramSocket socket = new DatagramSocket();

            byte[] data = command.getBytes();
            InetAddress mbotAddress = InetAddress.getByName("10.10.3.188");
            int mbotPort = 8888;

            DatagramPacket packet = new DatagramPacket(data, data.length, mbotAddress, mbotPort);
            socket.send(packet);

            System.out.println("Datagram sent to mBot.");

            socket.close();
        } catch (Exception e) {
            e.printStackTrace();
        }

        return "redirect:/";
    }
}
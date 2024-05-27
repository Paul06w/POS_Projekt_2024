package com.mongodb.starter;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.net.InetAddress;
import java.net.UnknownHostException;

@SpringBootApplication
public class ApplicationStarter {
    public static void main(String[] args) {
        String serverIp = "";
        try {
            String localIP = InetAddress.getLocalHost().getHostAddress();       //Ermitteln der IP-Adresse
            System.setProperty("local.ip", localIP);
            serverIp = localIP;
        } catch (UnknownHostException e) {
            e.printStackTrace();
            System.setProperty("local.ip", "127.0.0.1");                        //Loopback-Adresse
            serverIp = "127.0.0.1";
        }
        SpringApplication.run(ApplicationStarter.class, args);
        System.out.println("Server IP-Adresse: " + serverIp);
    }
}

'use strict';
const net = require('net');
const config = require('./config');

const ip = config.ip;
const port = config.port;

function handleInterrupt() {
  process.on('SIGINT', () => {
    server.close();
  });
}
let playerNum = 0;
let playerActions = ['', ''];
let playerSockets = [];
// main
const server = net.createServer((sock) => {
  if (playerNum < 2) {
    console.log(`Player connected from ${sock.remoteAddress}`);
    // player
    sock.write(playerNum + '\n');
    playerSockets[playerNum] = sock;
    playerNum++;
    sock.on('data', (data) => {
      const dataStr = data.toString('ascii').substring(0, data.length - 1);
      console.log('Received data:', dataStr);
      let d = dataStr.split('|');
      playerActions[d[0]] = d[1];
      // check if both players have registered
      if (playerActions[0] !== '' && playerActions[1] !== '') {
        //let playerActionStr = playerActions[0] + '|' + playerActions[1];
        playerSockets[0].write(playerActions[1] + '\n');
        playerSockets[1].write(playerActions[0] + '\n');
        playerActions[0] = '';
        playerActions[1] = '';
      }
    });  
    sock.on('close', (data) => {
      console.log('Player disconnected.');
    });  
  }
  else {
    console.log(`Administrator connected from ${sock.remoteAddress}`);
    // administrator
    sock.write('Welcome, administrator~\n');
    sock.on('data', (data) => {
      const dataStr = data.toString('ascii').substring(0, data.length - 1);
      if (dataStr === 'reset') {
        playerActions[0] = '';
        playerActions[1] = '';
        playerSockets[0].destroy();
        playerSockets[1].destroy();
        playerNum = 0;
        sock.write('Game reset!\n');
      }
    });
    sock.on('close', (data) => {
      console.log('Administrator disconnected.');
    });
  }
});

server.listen(port, ip);
console.log(`Server is listening on ${ip}:${port}`);
handleInterrupt();
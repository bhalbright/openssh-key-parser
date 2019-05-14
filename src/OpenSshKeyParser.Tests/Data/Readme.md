# Test key files

The test private keys were generated with PuTTYgen 0.71 on a Windows 10 machine and with ssh-keygen on Ubuntu 19.04. I have no reason to believe the keys would be different but wanted to test private keys generated from a couple different programs.

For PuTTYgen I simply chose the type of key, clicked Generate, and then exported as an OpenSSH key (in new format). For ssh-keygen, the commands I used are below. Note that early versions of ssh-keygen won't generate in the newer openssh file format (except for ED25519 I think). I had to upgrade Ubuntu to 19.04 to get the ssh-keygen that generates the open ssh key format by default.

RSA.txt:
```
ssh-keygen -t rsa -f RSA.txt -C 20190509_rsa
```
RSA.Encrypted.txt
```
ssh-keygen -t rsa -f RSA.Encrypted.txt -C 20190510_rsa_encrypted -N password
```

ECDSA.nistp256.txt
```
ssh-keygen -t ecdsa -b 256 -f ECDSA.nistp256.txt -C 201905011_ecdsa_nistp256
```

ECDSA.nistp256.Encrypted.txt
```
ssh-keygen -t ecdsa -b 256 -f ECDSA.nistp256.Encrypted.txt -C 201905011_ecdsa_nistp256 -N password
```

ECDSA.nistp384.txt
```
ssh-keygen -t ecdsa -b 384 -f ECDSA.nistp384.txt -C 201905011_ecdsa_nistp384
```

ECDSA.nistp384.Encrypted.txt
```
ssh-keygen -t ecdsa -b 384 -f ECDSA.nistp384.Encrypted.txt -C 201905011_ecdsa_nistp384 -N password
```

ECDSA.nistp521.txt
```
ssh-keygen -t ecdsa -b 521 -f ECDSA.nistp521.txt -C 201905011_ecdsa_nistp521
```

ECDSA.nistp521.Encrypted.txt
```
ssh-keygen -t ecdsa -b 521 -f ECDSA.nistp521.Encrypted.txt -C 201905011_ecdsa_nistp521 -N password
```

ED25519.txt
```
ssh-keygen -t ed25519 -f ED25519.txt -C 20190511_ed25519
```

ED25519.Encrypted.txt
```
ssh-keygen -t ed25519 -f ED25519.Encrypted.txt -C 20190511_ed25519 -N password
```

DSA.txt
```
ssh-keygen -t dsa -f DSA.txt -C 20190511_dsa
```

DSA.txt
```
ssh-keygen -t dsa -f DSA.Encrypted.txt -C 20190511_dsa_encrypted -N password
```

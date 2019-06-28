# openssh-key-parser
C# parser for private keys in the OpenSSH file format (i.e. openssh-key-v1).

The following key types are supported
* ssh-ed25519
* ssh-rsa
* ssh-dss (DSA)
* ecdsa-sha2-nistp256
* ecdsa-sha2-nistp384
* ecdsa-sha2-nistp521

Example usage:
```cs
var keyPair = (Ed25519KeyPair)OpenSshKeyParser.ParseOpenSshKeyFile(
	System.IO.File.ReadAllText(@"E:\keys\ed25519_with_passphrase"), "password");
```

The format for openssh-key-v1 is documented [here](https://github.com/openssh/openssh-portable/blob/master/PROTOCOL.key). But to be honest, this doesn't quite give you all you need. I looked quite a bit at how the [sshj](https://github.com/hierynomus/sshj) library parses these keys and a extracting a key/iv from the KDF was directly ported from sshj. The crypto stuff like Bcrypt and ae256-ctr were used from existing sources. I used a little code from [ssh.net](https://github.com/sshnet/SSH.NET) on reading the binary data from the keyfile. Finally, I owe picking the parts from ECDSA keys to this [ssh.net fork](https://github.com/darinkes/SSH.NET-1/tree/elliptic).

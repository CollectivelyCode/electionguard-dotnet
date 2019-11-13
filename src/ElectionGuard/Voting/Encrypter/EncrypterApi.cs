﻿using System;
using System.Runtime.InteropServices;
using ElectionGuard.SDK.Cryptography;
using ElectionGuard.SDK.KeyCeremony;
using ElectionGuard.SDK.Serialization;

namespace ElectionGuard.SDK.Voting.Encrypter
{
    internal struct EncrypterApi
    {
        [DllImport("electionguard", EntryPoint = "Voting_Encrypter_new")]
        internal static extern NewEncrypterReturn NewEncrypter(UniqueIdentifier uniqueIdentifier, JointPublicKey jointKey, uint numberOfSelections, [MarshalAs(UnmanagedType.LPArray, SizeConst = CryptographySettings.HashDigestSizeBytes)] byte[] baseHash);

        [DllImport("electionguard", EntryPoint = "Voting_Encrypter_free")]
        internal static extern void FreeEncrypter(UIntPtr encrypter);

        [DllImport("electionguard", EntryPoint = "Voting_Encrypter_encrypt_ballot")]
        internal static extern EncryptBallotReturn EncryptBallot (UIntPtr encrypter, bool[] selections);

        internal static UniqueIdentifier NewUniqueIdentifier()
        {
            var guid = new Guid().ToByteArray();
            var uniqueId = Marshal.AllocHGlobal(guid.Length);
            Marshal.Copy(guid, 0, uniqueId, guid.Length);
            return new UniqueIdentifier()
            {
                Length = guid.LongLength,
                Bytes = uniqueId,
            };
        }

        internal static void FreeUniqueIdentifier(IntPtr uniqueIdentifier)
        {
            Marshal.FreeHGlobal(uniqueIdentifier);
        }

        internal static JointPublicKey NewJointPublicKey(JointPublicKeyResponse jointPublicKeyResponse)
        {
            var unmanagedPointer = Marshal.AllocHGlobal(jointPublicKeyResponse.Length);
            Marshal.Copy(JointKeySerializer.Serialize(jointPublicKeyResponse), 0, unmanagedPointer, jointPublicKeyResponse.Length);
            return new JointPublicKey()
            {
                Length = jointPublicKeyResponse.Length,
                Bytes = unmanagedPointer,
            };
        }

        internal static void FreeJointPublicKey(JointPublicKey jointPublicKey)
        {
            Marshal.FreeHGlobal(jointPublicKey.Bytes);
        }
    }
}
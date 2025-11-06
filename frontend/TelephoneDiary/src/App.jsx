import { useEffect, useState } from "react";
import {
  Container,
  Button,
  TextField,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  IconButton,
} from "@mui/material";
import { Delete, Edit } from "@mui/icons-material";
import axios from "axios";

const API_URL = import.meta.env.VITE_API_URL;

export default function App() {
  const [contatos, setContatos] = useState([]);
  const [open, setOpen] = useState(false);
  const [editContato, setEditContato] = useState(null);
  const [search, setSearch] = useState("");

  const [formData, setFormData] = useState({
    nome: "",
    idade: "",
    telefones: [{ id: null, numero: "" }],
  });

const fetchContatos = async () => {
  try {
    const response = await axios.get(API_URL);
    setContatos(response.data);
  } catch (error) {
    if (error.response && error.response.status === 404) {
      setContatos([]);
      return;
    }
    console.error("Erro ao buscar contatos:", error);
  }
};


  useEffect(() => {
    fetchContatos();
  }, []);

  const handleAddTelefone = () => {
    setFormData({
      ...formData,
      telefones: [...formData.telefones, { id: null, numero: "" }],
    });
  };

  const handleTelefoneChange = (value, index) => {
    const phones = [...formData.telefones];
    phones[index].numero = value;
    setFormData({ ...formData, telefones: phones });
  };

const handleSave = async () => {

  const idadeNum = Number(formData.idade);
  if (!Number.isInteger(idadeNum) || idadeNum < 0 || idadeNum > 150) {
    alert("A idade deve ser um número entre 0 e 150!");
    return;
  }

  const telefonesValidos = formData.telefones
    .map(t => ({
      id: t.id || undefined,
      numero: t.numero.trim()
    }))
    .filter(t => t.numero !== "");

  const numeros = telefonesValidos.map(t => t.numero);
  const numerosUnicos = [...new Set(numeros)];

  if (numerosUnicos.length !== numeros.length) {
    alert("Já existe um telefone igual para este contato!");
    return;
  }

  const payload = {
    nome: formData.nome.trim(),
    idade: idadeNum,
    telefones: telefonesValidos
  };

  try {
    if (editContato) {
      await axios.put(`${API_URL}/${editContato.id}`, payload);
    } else {
      await axios.post(API_URL, payload);
    }

    await fetchContatos();
    setOpen(false);
    resetForm();
  } catch (err) {
    console.error("Erro ao salvar:", err.response?.data || err);
    alert("Erro ao salvar contato");
  }
};




  const resetForm = () => {
  setFormData({
    nome: "",
    idade: "",
    telefones: [{ numero: "" }]
  });
  setEditContato(null);
};


  const handleEdit = (contato) => {
    setEditContato(contato);
    setFormData({
      nome: contato.nome,
      idade: contato.idade,
      telefones: contato.telefones.map((t) => ({
        id: t.id,
        numero: t.numero,
      })),
    });
    setOpen(true);
  };

  const handleDelete = async (id) => {
    if (!confirm("Tem certeza que deseja excluir?")) return;

    await axios.delete(`${API_URL}/${id}`);
    fetchContatos();
  };

const filteredContatos = contatos.filter((c) => {
  const matchNome = c.nome?.toLowerCase().includes(search.toLowerCase());
  const matchTelefone = Array.isArray(c.telefones)
    ? c.telefones.some((t) =>
        t.numero?.toLowerCase().includes(search.toLowerCase())
      )
    : false;

  return matchNome || matchTelefone;
});


  return (
    <Container>
      <h1>Agenda Telefônica</h1>

      <Button variant="contained" onClick={() => setOpen(true)}>
        + Adicionar Contato
      </Button>

      <TextField
        label="Pesquisar por nome/telefone"
        variant="outlined"
        fullWidth
        style={{ margin: "20px 0" }}
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />

      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Nome</TableCell>
            <TableCell>Idade</TableCell>
            <TableCell>Telefones</TableCell>
            <TableCell>Ações</TableCell>
          </TableRow>
        </TableHead>

        <TableBody>
          {filteredContatos.length > 0 ? (
            filteredContatos.map((c) => (
              <TableRow key={c.id}>
                <TableCell>{c.nome}</TableCell>
                <TableCell>{c.idade}</TableCell>
                <TableCell>
                 {Array.isArray(c.telefones) && c.telefones.length > 0
  ? c.telefones.map((t) => t.numero).join(", ")
  : "—"}
                </TableCell>

                <TableCell>
                  <IconButton onClick={() => handleEdit(c)}>
                    <Edit />
                  </IconButton>

                  <IconButton onClick={() => handleDelete(c.id)}>
                    <Delete style={{ color: "red" }} />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))
          ) : (
            <TableRow>
              <TableCell colSpan="4" align="center">
                Nenhum contato encontrado
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>

      {/* Modal */}
      <Dialog open={open} onClose={() => setOpen(false)}>
        <DialogTitle>
          {editContato ? "Editar Contato" : "Novo Contato"}
        </DialogTitle>

        <DialogContent>
          <TextField
            label="Nome"
            fullWidth
            margin="dense"
            value={formData.nome}
            onChange={(e) =>
              setFormData({ ...formData, nome: e.target.value })
            }
          />

          <TextField
            label="Idade"
            type="number"
            fullWidth
            margin="dense"
            value={formData.idade}
            onChange={(e) =>
              setFormData({ ...formData, idade: e.target.value })
            }
          />

          {formData.telefones.map((t, i) => (
            <TextField
              key={i}
              label={`Telefone ${i + 1}`}
              fullWidth
              margin="dense"
              value={t.numero}
              onChange={(e) => handleTelefoneChange(e.target.value, i)}
            />
          ))}

          <Button onClick={handleAddTelefone}>+ Telefone</Button>
        </DialogContent>

        <DialogActions>
          <Button onClick={() => {
            setOpen(false);
            resetForm();
          }}>
            Cancelar
          </Button>

          <Button variant="contained" onClick={handleSave}>
            Salvar
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
}

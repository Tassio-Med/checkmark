import React, { useState } from 'react';
import CheckmarkList from './components/CheckmarkList/CheckmarkList';
import CheckmarkForm from './components/CheckmarkForm/CheckmarkForm';
import { Checkmark, PriorityLevel } from './models/Checkmark';
import './App.css';

type ViewType = 'list' | 'create' | 'edit';

const App: React.FC = () => {
  const [currentView, setCurrentView] = useState<ViewType>('list');
  const [checkmarkToEdit, setCheckmarkToEdit] = useState<Checkmark | null>(null);

  const navigateTo = (view: ViewType, checkmark?: Checkmark) => {
    setCurrentView(view);
    if (checkmark) {
      setCheckmarkToEdit(checkmark);
    } else {
      setCheckmarkToEdit(null);
    }
  };

  const handleCheckmarkSaved = () => {
    setCurrentView('list');
    setCheckmarkToEdit(null);
  };

  const handleCancel = () => {
    setCurrentView('list');
    setCheckmarkToEdit(null);
  };

  return (
    <div className="App">
      <header className="App-header">
        <h1>✅ Checkmark App</h1>
        <p>Gerencie suas tarefas de forma simples e eficiente</p>
      </header>

      <nav className="App-nav">
        <button 
          className={`nav-btn ${currentView === 'list' ? 'active' : ''}`}
          onClick={() => navigateTo('list')}
        >
          📋 Lista de Checkmarks
        </button>
        
        <button 
          className={`nav-btn ${currentView === 'create' ? 'active' : ''}`}
          onClick={() => navigateTo('create')}
        >
          ➕ Novo Checkmark
        </button>
      </nav>

      <main className="App-main">
        {currentView === 'list' && (
          <CheckmarkList 
            onEditCheckmark={(checkmark) => navigateTo('edit', checkmark)}
          />
        )}

        {currentView === 'create' && (
          <div className="form-container">
            <h2>Criar Novo Checkmark</h2>
            <CheckmarkForm 
              onSave={handleCheckmarkSaved}
              onCancel={handleCancel}
            />
          </div>
        )}

        {currentView === 'edit' && checkmarkToEdit && (
          <div className="form-container">
            <h2>Editar Checkmark</h2>
            <CheckmarkForm 
              checkmark={checkmarkToEdit}
              onSave={handleCheckmarkSaved}
              onCancel={handleCancel}
            />
          </div>
        )}
      </main>

      <footer className="App-footer">
        <p>Checkmark App - Desenvolvido com React, TypeScript e .NET 8</p>
      </footer>
    </div>
  );
};

export default App;